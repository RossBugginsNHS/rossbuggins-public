namespace CommsCheck;

public class RulesEngineLoaderHostedService(
    RulesEngineRulesLoader _loader, 
    RuleEngineFactory _factory) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _factory.RulesEngine = await _loader.LoadRulesEngine();
        await base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
         _factory.RulesEngine = null;
        return base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}