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

if(args.Contains("--benchmark") && args.Contains("--debug"))
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
        options.MapType<DateOnly>(() => new OpenApiSchema()
        {
            Type = "string",
            Format = "date"
        });
    }
    );

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapPrometheusScrapingEndpoint();

app.MapPost("/check", 
    async Task<Results<
            AcceptedAtRoute<CommsCheckQuestionResponseDto>, 
            CreatedAtRoute<CommsCheckQuestionResponseDto>>> (
        [FromBody] CommsCheckQuestionRequestDto request,
        [FromServices] IDistributedCache cache,
        [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new CheckCommsCommand(request));

            var itemBytes = await cache.GetAsync(result.ResultId);
            if(itemBytes==null)
                return TypedResults.CreatedAtRoute(
                    result,
                    "CommCheckResult",
                    new { resultId = result.ResultId });

            return TypedResults.AcceptedAtRoute(
                    result,
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

app.MapGet("/rules", 
    async (
        [FromServices] IOptions<CommsCheckRulesEngineOptions> options) =>
    {
        var str = await File.ReadAllTextAsync(options.Value.JsonPath);
        var rules = System.Text.Json.JsonSerializer.Deserialize<object>(str);
        return TypedResults.Json(rules);
    });

app.Run();