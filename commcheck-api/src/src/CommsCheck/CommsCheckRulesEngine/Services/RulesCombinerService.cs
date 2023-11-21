namespace CommsCheck;

using System.Collections.Concurrent;
using MediatR;

public class RulesCombinerService
{
    private readonly IPublisher _publisher;
    private readonly ConcurrentDictionary<Guid, List<IRuleOutcome>> _outcomes;
    private readonly ConcurrentDictionary<Guid, List<RuleResultSummary>> _summaries;

    public RulesCombinerService(IPublisher publisher)
    {
        _publisher = publisher;
        _outcomes = new ConcurrentDictionary<Guid, List<IRuleOutcome>>();
        _summaries = new ConcurrentDictionary<Guid, List<RuleResultSummary>>();
    }

    public async Task Combine(RulesRunCompletedEvent notification, int outcomeLimit)
    {
        var outcomes = _outcomes.AddOrUpdate(
            notification.RuleRunId,
            new List<IRuleOutcome> { notification.Outcome },
            (id, existing) => new List<IRuleOutcome> { notification.Outcome }.Concat(existing).ToList());

        var summaries = _summaries.AddOrUpdate(
             notification.RuleRunId,
             notification.RuleSummaries,
             (id, existing) => notification.RuleSummaries.Concat(existing).ToList());
        
        await CheckIfAllThere(notification, outcomeLimit, outcomes, summaries);
    }

    private async Task CheckIfAllThere(
        RulesRunCompletedEvent notification,
        int outcomeLimit,
        List<IRuleOutcome> outcomes,
        List<RuleResultSummary> summaries)
    {

        //bit naive here. What should it know about how many rule checks happen?
        if (outcomes.Count >= outcomeLimit)
        {
            await _publisher.Publish(new RuleResultsCombinedEvent(
              notification.CommCheckCorrelationId,
              notification.RuleRunId,
              notification.RulesHash,
              notification.Method,
              notification.ToCheck,
              outcomes,
              summaries));

            _outcomes.Remove(notification.RuleRunId, out _);
            _summaries.Remove(notification.RuleRunId, out _);
        }
    }
}
