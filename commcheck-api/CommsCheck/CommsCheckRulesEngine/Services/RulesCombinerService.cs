namespace CommsCheck;

using System.Collections.Concurrent;
using MediatR;

public class RulesCombinerService(IPublisher _publisher)
{
    ConcurrentDictionary<Guid, List<IRuleOutcome>> _outcomes = new ConcurrentDictionary<Guid, List<IRuleOutcome>>();
    public Task Combine(RulesRunCompleteEvent notification)
    {
        var outcomes = _outcomes.AddOrUpdate(
            notification.RuleRunId,
            new List<IRuleOutcome> { notification.Outcome },
            (id, existing) => new List<IRuleOutcome> { notification.Outcome }.Concat(existing).ToList());

        CheckIfAllThere(notification, outcomes);

        return Task.CompletedTask;
    }

    private Task CheckIfAllThere(RulesRunCompleteEvent notification, List<IRuleOutcome> outcomes)
    {
        //bit naive here. What should it know about how many rule checks happen?
        if (outcomes.Count == 3)
        {
            _publisher.Publish(new RuleResultsCombinedEvent(
              notification.RuleRunId,
              notification.Method,
              notification.ToCheck,
              outcomes));
            
            _outcomes.Remove(notification.RuleRunId, out _);
        }
        return Task.CompletedTask;
    }
}
