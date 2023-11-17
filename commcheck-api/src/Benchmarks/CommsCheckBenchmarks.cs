namespace CommsCheck.Benchmarks;

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using CommsCheck;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class CommsCheckBenchmarks
{
    IHost? app;

    [ParamsSource(nameof(ValuesForA))]
    public int CheckCount { get; set; }

    public IEnumerable<int> ValuesForA => Vals();

    private static IEnumerable<int> Vals()
    {
        yield return 1;
        yield return 10;
        yield return 100;
        yield return 1000;
    }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        app = Host.CreateDefaultBuilder()
               .ConfigureLogging(config =>
               {
                   config.AddFilter("CommsCheck", LogLevel.Warning);
               })
               .ConfigureServices((context, services) =>
               {
                   services
                    .AddCommsCheck(options =>
                    {
                        options
                            .AddJsonConfig()
                            .AddDistriubtedCache()
                            .AddShaKey("dfgklretlk345dfgml12")
                            .AddMetrics()
                            .AddRulesEngineRules(
                                context.Configuration.GetSection("CommsCheck"),
                                ruleEngineOptions =>
                                {
                                    ruleEngineOptions
                                        .AddContactType<Sms>()
                                        .AddContactType<Email>()
                                        .AddContactType<Postal>()
                                        .AddContactType<App>();
                                });
                    });
                   services.AddSingleton<IDistributedCache, ClearableMemoryDistributedCache>();
               }).Build();
        await app.StartAsync();
    }

    [IterationSetup]
    public virtual void IterationSetup()
    {
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        ArgumentNullException.ThrowIfNull(app);
        var cache = app.Services.GetRequiredService<IDistributedCache>();
        ClearableMemoryDistributedCache dc = (ClearableMemoryDistributedCache)cache;
        dc.Clear();
    }

    [Benchmark]
    public async Task RunBenchmark()
    {
        ArgumentNullException.ThrowIfNull(app);
        var cache = app.Services.GetRequiredService<IDistributedCache>();
        ArgumentNullException.ThrowIfNull(cache);
        var sender = app.Services.GetService<ISender>();
        ArgumentNullException.ThrowIfNull(sender);
        var startAt = GetStartDate();

        await SendRequests(sender, cache, startAt);
    }

    public DateTime GetStartDate() => new DateTime(1800, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public async Task SendRequests(ISender sender, IDistributedCache cache, DateTime startAt)
    {
        for (int i = 0; i < CheckCount; i++)
        {
            await SendSingleRequest(sender, cache, startAt, i);
        }
    }

    public async Task SendSingleRequest(ISender sender, IDistributedCache cache, DateTime startAt, int i)
    {
        var thisDate = startAt.AddDays(i);
        var request = BuildRequestDto(thisDate);
        await SendAndWait(sender, cache, request);
    }

    public async Task SendAndWait(ISender sender, IDistributedCache cache, CommsCheckQuestionRequestDto request)
    {
        var x = await sender.Send(new CheckCommsCommand(request));
        await WaitForResult(x.ResultId, cache);
    }

    public CommsCheckQuestionRequestDto BuildRequestDto(DateTime thisDate) =>
        new CommsCheckQuestionRequestDto(
            DateOnly.FromDateTime(thisDate),
             DateOnly.FromDateTime(thisDate),
             null);

    public async Task WaitForResult(string resultId, IDistributedCache cache)
    {
        byte[]? data = null;
        while (ContinueToWait(data))
        {
            data = await TryGetResult(resultId, cache);
        }
    }

    public bool ContinueToWait(byte[]? data) => data == null;

    public async Task<byte[]?> TryGetResult(string resultId, IDistributedCache cache)
    {
        var data = await cache.GetAsync(resultId);
        if (data == null)
            await Task.Yield();
        return data;
    }
}