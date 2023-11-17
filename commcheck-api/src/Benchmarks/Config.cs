namespace CommsCheck.Benchmarks;
using BenchmarkDotNet.Configs;

public class Config : ManualConfig
{
    public Config()
    {
        AddColumn(new TagColumn("MeanPerCheck",
        (summary, benchmarkCase) =>
            (benchmarkCase.Descriptor.ToString())));
    }
}
