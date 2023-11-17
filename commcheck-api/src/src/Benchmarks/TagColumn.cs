namespace CommsCheck.Benchmarks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

public class TagColumn : IColumn
{
    private readonly Func<Summary, BenchmarkCase, string> getTag;

    public string Id { get; }
    public string ColumnName { get; }

    public TagColumn(string columnName, Func<Summary, BenchmarkCase, string> getTag)
    {
        this.getTag = getTag;
        ColumnName = columnName;
        Id = nameof(TagColumn) + "." + ColumnName;
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => getTag(
        summary, benchmarkCase);

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        string? logicalGroupKey = summary.GetLogicalGroupKey(benchmarkCase);
        ArgumentNullException.ThrowIfNull(logicalGroupKey);
        var currentStat = summary[benchmarkCase].ResultStatistics;
        ArgumentNullException.ThrowIfNull(currentStat);
        var param = benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "CheckCount");
        ArgumentNullException.ThrowIfNull(param);

        var loop = (int)param.Value;
        return Math.Round((currentStat.Mean / (double)1000000 / (double)loop), 2).ToString() + "ms";
    }
    
    public bool IsAvailable(Summary summary) => true;
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => false;
    public UnitType UnitType => UnitType.Time;
    public string Legend => $"Custom '{ColumnName}' tag column";

    public override string ToString() => ColumnName;
}
