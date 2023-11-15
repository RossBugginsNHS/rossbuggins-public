namespace CommsCheck;

using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;
using RulesEngine;
using RulesEngine.Models;

public class CommsCheckRulesEngine : ICommCheck
{
    private readonly RulesEngine _rulesEngine;
    private readonly ILogger<CommsCheckRulesEngine> _logger;
    private readonly IOptions<CommsCheckRulesEngineOptions> _options;
    private readonly IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> _rules;
    private readonly IPublisher _publisher;
    public CommsCheckRulesEngine(
         ILogger<CommsCheckRulesEngine> logger,
         IOptions<CommsCheckRulesEngineOptions> options,
         IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> rules,
         IPublisher publisher)
    {
        _logger = logger;
        _options = options;
        _rules = rules;
        _publisher = publisher;
        
        _rulesEngine =LoadRulesEngineFromFile (_options.Value.JsonPath);
    }

    private RulesEngine LoadRulesEngineFromFile(string fileName)
    {
        _logger.LogInformation("Loading rules engine from {fileLocation}", fileName);
        var fileData = File.ReadAllText(_options.Value.JsonPath);
        var rules = LoadRulesEngine(fileData);
        _logger.LogInformation("Loaded rules engine from {fileLocation}", fileName);
        return rules;

    }
    private RulesEngine LoadRulesEngine(string fileData)
    {
        var workflow = System.Text.Json.JsonSerializer.Deserialize<List<Workflow>>(fileData);
        if (workflow == null)
            throw new SerializationException($"Workflow failed to deserialize from {_options.Value.JsonPath}");
        var rulesEngine = new RulesEngine(workflow.ToArray());
        return rulesEngine;
    }

    public async Task Check(CommsCheckItemWithId toCheck)
    {
        await Parallel.ForEachAsync(_rules, async (rule, ct) =>
        {
            await _publisher.Publish(new RunRuleEvent(rule, _rulesEngine, toCheck));
        });
    }
}
