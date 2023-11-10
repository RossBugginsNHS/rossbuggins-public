using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text.Json;
using System.Text;
using System.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommsCheck(options =>
    {
        options
            .AddJsonConfig()
            .AddDistriubtedCache()
            .AddShaKey("dfgklretlk345dfgml12");

        //This is a version that used c# classes for rules, instead of the json rules
        //options.AddNativeRules(nativeRules =>
            //{
            //    nativeRules 
            //        .AddRule<DeaExplicitBlockRule>()
            //        .AddRule<Over115ExplicitRule>()
            //        .AddRule<Sms, SmsCgaRule>()
            //        .AddRule<Sms, SmsNoReasonfRemovalRule>();
            //})

        options.AddRulesEngineRules(
            builder.Configuration.GetSection("CommsCheck"),
            ruleEngineOptions =>
            {
                //The path to the rules file. Set by default in app settings.json
               // ruleEngineOptions.RulesPath = "./rules.json";
            });

           
    }
);

builder.Services.AddTransient<ICommCheck,CommsCheckRulesEngine>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
