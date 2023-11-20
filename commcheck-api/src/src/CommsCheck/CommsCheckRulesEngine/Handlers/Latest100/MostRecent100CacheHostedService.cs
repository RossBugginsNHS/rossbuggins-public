namespace CommsCheck;

public class MostRecent100CacheHostedService(MostRecent100Cache cache) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach(var item in cache.GetReader().ReadAllAsync(stoppingToken))
        {
            var existingList = cache.Get().TakeLast(99);
            var newList = new List<CommsCheckAnswerResponseDto>();
            newList.AddRange(existingList);
            newList.Add(item);
            cache.UpdateData(newList);
        }
    }
}