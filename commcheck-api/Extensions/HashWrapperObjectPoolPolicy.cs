namespace CommsCheck;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.ObjectPool;

public class HashWrapperObjectPoolPolicy(IServiceProvider sp)
: PooledObjectPolicy<HashWrapper>
{
    private static readonly Meter MyMeter = new("NHS.CommChecker.HashWrapperObjectPoolPolicy", "1.0");
    private static readonly Counter<long> CreatedCounter = MyMeter.CreateCounter<long>("HashWrapper_Created_Count");
    private static readonly Counter<long> ReturnedCounter = MyMeter.CreateCounter<long>("HashWrapper_Returned_Count");
    public override HashWrapper Create()
    {
        CreatedCounter.Add(1);
        return ActivatorUtilities.GetServiceOrCreateInstance<HashWrapper>(sp);
    }

    public override bool Return(HashWrapper obj)
    {
        ReturnedCounter.Add(1);
        return true;
    }
}
