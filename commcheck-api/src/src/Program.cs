using CommsCheck;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Options;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using CommsCheck.Benchmarks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

if (args.Contains("--benchmark") && args.Contains("--debug"))
{
    var bm = new CommsCheckBenchmarks();
    await bm.GlobalSetup();
    await bm.RunBenchmark();
    bm.IterationCleanup();
    return;
}
else if (args.Contains("--benchmark"))
{
    BenchmarkRunner.Run<CommsCheckBenchmarks>();
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRulesEngine();
builder.Services.AddSingleton<CommsCheckItemFactory>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddRule100();

builder.Services.AddCommsCheck(options =>
    {
        options
            .AddJsonConfig()
            .AddDistriubtedCache()
            .AddShaKey("dfgklretlk345dfgml12")
            .AddMetrics()

            .AddRulesEngineRules(
                builder.Configuration.GetSection("CommsCheck"),
                ruleEngineOptions =>
                {
                    ruleEngineOptions
                        .AddContactType<Sms>()
                        .AddContactType<Email>()
                        .AddContactType<Postal>()
                        .AddContactType<App>()
                        ;
                });
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        var desc = """
        Rules engine for providing a standardised and central decision tree for deciding 
        if there should be communication with a person.

        This API makes its decision based on the data provided in the request payload, and does
        not use data sources directly.

        - Submit data using the POST /check endpoint
        - The response contains a Location header with a url and unique id
        - Retrieve answer from GET /check/results/{resultId} endpoint

        Available at the readme github repo is source for a cli tool for directly calling the api.

        - GET <a href="/check/results">/check/results</a> provides a list of the last 100 queries
        - GET <a href="/check/results/stream">/check/results/stream</a> can be used to provide a realtime stream of requests.

        Notes for data payload:
        - Relative Date - this is used to calculate any date spans, for example Age based on Date of birth. 
        - Dob will be always set to first day of the month.
        - Postcode will be set to first 4 characters of postcode.

        Notes for API:
        - The api is deterministic. It will always give the same result based on input data (as long as the rule set is the same)
        - Metrics for the API can be found at GET <a href="/metrics">/metrics</a> endpoint.
        - View current rules at GET <a href="/rules">/rules</a> endpoint.
        """;

 
        options.AddDateAndEnumFormatters();
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Comms Check API v1",
            Description = desc,
             
            Contact = new OpenApiContact
            {
                Name = "Readme",
                Url = new Uri("https://github.com/RossBugginsNHS/rossbuggins-public/blob/master/commcheck-api/readme.md")
            }
        });
    }
    );

var app = builder.Build();
app.UseSwagger(option =>
    {
        option.RouteTemplate = "{documentName}/swagger.json";
    });
app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/v1/swagger.json", "Comms Check API");
        option.RoutePrefix = "";
    });
app.MapPrometheusScrapingEndpoint();

app.MapPost("/check",
    async Task<Results<
            AcceptedAtRoute<CommsCheckQuestionResponseDto>,
            CreatedAtRoute<CommsCheckQuestionResponseDto>>> (
        [FromBody] CommsCheckQuestionRequestDto request,
        HttpContext http,
        [FromServices] IDistributedCache cache,
        [FromServices] ISender sender) =>
        {
            var filteredRequest = request with 
            {
                DateOfBirth = new DateOnly(
                    request.DateOfBirth.Year,
                    request.DateOfBirth.Month,
                    1),
                PostCode = request.PostCode.DistrictOnly()
            };

            var result = await sender.Send(new CheckCommsCommand(filteredRequest));
            var itemBytes = await cache.GetAsync(result.ResultId);
            if (itemBytes == null)
            {
                var retryAfter = 1;
                http.Response.Headers.RetryAfter = retryAfter.ToString();
                return TypedResults.AcceptedAtRoute(
                    result with { RetryAfter = retryAfter },
                    "CommCheckResult",
                    new { resultId = result.ResultId });
            }
            return TypedResults.CreatedAtRoute(
                    result with { RetryAfter = 0 },
                    "CommCheckResult",
                    new { resultId = result.ResultId });
        }
 )
.WithName("CommCheck")
.WithOpenApi();

app.MapGet("/check/result/{resultId}",
    async Task<Results<Ok<CommsCheckAnswerResponseDto>, NotFound>> (
        string resultId,
        [FromServices] IDistributedCache cache) =>
        {
            var itemBytes = await cache.GetAsync(resultId);
            if (itemBytes == null)
                return TypedResults.NotFound();

            var itemStr = JsonSerializer.Deserialize<CommsCheckAnswer>(itemBytes);
            var itemDto = CommsCheckAnswerResponseDto.FromCommsCheckAnswer(itemStr);
            return TypedResults.Ok(itemDto);
        }
 )
.WithName("CommCheckResult")
.WithOpenApi();

app.MapGet("/check/results",
      async Task<Results<Ok<IEnumerable<CommsCheckAnswerResponseDto>>, NotFound>> (
        [FromServices] MostRecent100Cache cache) =>
        {
            await Task.Yield();
            var items = cache.Get().Reverse();
            if (items == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(items);
        }
 )
.WithName("CommCheckResults")
.WithOpenApi();

app.MapGet("/check/results/stream",
        async ([FromServices] MostRecent100Cache cache, CancellationToken ct) =>
        {
            await Task.Yield();
            async IAsyncEnumerable<CommsCheckAnswerResponseDto> Str(
                [EnumeratorCancellation] CancellationToken cancellationToken)
            {
                await foreach (var item in cache.GetStream(cancellationToken))
                {
                    yield return item;
                }
            }

            return Str(ct);
        }
 )
.WithName("CommCheckResultsStream")
.WithOpenApi();

app.MapGet("/rules",
    async (
        [FromServices] IOptions<CommsCheckRulesEngineOptions> options) =>
    {
        var str = await File.ReadAllTextAsync(options.Value.JsonPath);
        var rules = System.Text.Json.JsonSerializer.Deserialize<object>(str);
        return TypedResults.Json(rules);
    });

app.Run();