using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using CommsCheck;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;


[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class CommsCheckBenchmarks
{
    IHost? app;

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
    public void IterationSetup()
    {

    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        if (app == null)
            throw new NullReferenceException(nameof(app));

        var cache = app.Services.GetRequiredService<IDistributedCache>();
        ClearableMemoryDistributedCache dc = (ClearableMemoryDistributedCache)cache;
        dc.Clear();
    }


      private class Config : ManualConfig
        {
            public Config()
            {
                AddColumn(new TagColumn("MeanPerLoop", 
                (summary, benchmarkCase) => 
                    (benchmarkCase.Descriptor.ToString())));
            }
        }

         public class TagColumn : IColumn
    {
        private readonly Func<Summary, BenchmarkCase,string> getTag;

        public string Id { get; }
        public string ColumnName { get; }

        public TagColumn(string columnName, Func<Summary, BenchmarkCase,string> getTag)
        {
            this.getTag = getTag;
            ColumnName = columnName;
            Id = nameof(TagColumn) + "." + ColumnName;
        }

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => getTag(
            summary, benchmarkCase);

        public bool IsAvailable(Summary summary) => true;
        public bool AlwaysShow => true;
        public ColumnCategory Category => ColumnCategory.Custom;
        public int PriorityInCategory => 0;
        public bool IsNumeric => false;
        public UnitType UnitType => UnitType.Time;
        public string Legend => $"Custom '{ColumnName}' tag column";
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) 
        { 
             string? logicalGroupKey = summary.GetLogicalGroupKey(benchmarkCase);

             if(logicalGroupKey==null)
                throw new Exception();

             var currentStat = summary[benchmarkCase].ResultStatistics;
             var param = benchmarkCase.Parameters.Items.FirstOrDefault(x=>x.Name == "LoopCount");
             if(param==null)
                throw new Exception("param is null");
             var loop =(int)param.Value;

             if(currentStat==null)
                throw new Exception();

             return Math.Round((currentStat.Mean / (double)1000000/ (double)loop),2).ToString() + "ms";
           // GetValue(summary, benchmarkCase);
        }
        public override string ToString() => ColumnName;
    }
        
    [ParamsSource(nameof(ValuesForA))]
    public int LoopCount { get; set; }

    public IEnumerable<int> ValuesForA =>Vals();

    private  IEnumerable<int> Vals()
    {
         yield return 1;
         yield return 10;
         yield return 100;
         yield return 1000;
    }

    [Benchmark]
    public async Task RunBenchmark()
    {
        if (app == null)
            throw new NullReferenceException(nameof(app));

        var cache = app.Services.GetRequiredService<IDistributedCache>();

        var incrementBy = (int)LoopCount;
        if (app == null)
            throw new ArgumentNullException(nameof(app));
        var sender = app.Services.GetService<ISender>();

        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        var startAt = new DateTime(1800, 1, 1);

        for (int i = 0; i < LoopCount; i++)
        {
            var thisDate = startAt.AddDays(i);
            var request = new CommsCheckQuestionRequestDto(
                DateOnly.FromDateTime(thisDate),
                 DateOnly.FromDateTime(thisDate),
                 null);
            var x = await sender.Send(new CheckCommsCommand(request));

            byte[]? data = null;

            while (data == null)
            {
                data = await cache.GetAsync(x.ResultId);
                if(data==null)
                    await Task.Yield();
            }

            var dataLength = data.Length;
        }


    }
}