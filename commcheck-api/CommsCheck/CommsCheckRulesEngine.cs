using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RulesEngine.Models;

public class CommsCheckRulesEngine : ICommCheck
{
    private readonly RulesEngine.RulesEngine _rulesEngine;
    private readonly ILogger<CommsCheckRulesEngine> _logger;
    private readonly IOptions<CommsCheckRulesEngineOptions> _options;

    private readonly IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> _rules;
    public CommsCheckRulesEngine(
         ILogger<CommsCheckRulesEngine> logger,
         IOptions<CommsCheckRulesEngineOptions> options,
         IEnumerable<ICommsCheckRulesEngineRuleRun<IContactType>> rules)
    {

        _logger = logger;
        _options = options;
        _rules = rules;
        var fileData = File.ReadAllText(_options.Value.JsonPath);
        _rulesEngine = LoadRulesEngine(fileData);

    }

    private RulesEngine.RulesEngine LoadRulesEngine(string fileData)
    {
        var workflow = System.Text.Json.JsonSerializer.Deserialize<List<Workflow>>(fileData);
        if (workflow == null)
            throw new Exception($"Workflow failed to deserialize from {_options.Value.JsonPath}");
        var rulesEngine = new RulesEngine.RulesEngine(workflow.ToArray());
        return rulesEngine;
    }

    public async Task<CommsCheckAnswer> Check(CommsCheckItemWithId toCheck)
    {
        //rules engine should be thread safe
        //https://github.com/microsoft/RulesEngine/discussions/438

        var cb = new ConcurrentBag<IRuleOutcome>();
        
        await Parallel.ForEachAsync(_rules, async (rule, ct) => { 
            var result = await rule.Run(_rulesEngine, toCheck.Item);
            cb.Add(result);
            });


        return new CommsCheckAnswer(
            toCheck.Id,
            toCheck.Item.ToString(),
            cb.ToArray());
    }



}
