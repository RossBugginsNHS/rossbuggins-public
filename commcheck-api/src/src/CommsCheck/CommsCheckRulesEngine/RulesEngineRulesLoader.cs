namespace CommsCheck;

using System.Data;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using RulesEngine;
using RulesEngine.Models;

public class RulesEngineAndHash(RulesEngine rulesEngine, string rulesHash)
{
    public RulesEngine RulesEngine => rulesEngine;
    public string RulesHash => rulesHash;

}
public class RulesEngineRulesLoader
{
    public const string CommsCheckRuleEngineCacheKey = "CommsCheckRulesEngineFileData";

    private readonly IDistributedCache _cache;
    private readonly ILogger<RulesEngineRulesLoader> _logger;
    private readonly IOptions<CommsCheckRulesEngineOptions> _options;

    public RulesEngineRulesLoader(
         IDistributedCache cache,
         ILogger<RulesEngineRulesLoader> logger,
         IOptions<CommsCheckRulesEngineOptions> options)
    {
        _cache = cache;
        _logger = logger;
        _options = options;
    }

    public async Task<RulesEngineAndHash> LoadRulesEngine()
    {
        return await LoadRulesEngine(_options.Value.JsonPath);
    }

    private async Task<string> LoadData(string fileName)
    {
        var fileData = await TryGetContentsFromCache();
        if (IsContentsLoadedFromCache(fileData))
        {
            _logger.LogInformation("Loaded rules engine contents from cache.");
        }
        else
        {
            fileData = await LoadRulesContentFromFile(fileName);
            await SaveToCache(fileData);
        }

        if (fileData == null)
        {
            throw new NoNullAllowedException("Rules engine contents is null.");
        }

        return fileData;
    }

    private static bool IsContentsLoadedFromCache(string? fileData) => fileData != null;

    private async Task<string?> TryGetContentsFromCache()
    {
        _logger.LogInformation("Looking for rules engine contents in cache.");
        return await _cache.GetStringAsync(CommsCheckRuleEngineCacheKey);
    }
    private async Task SaveToCache(string fileData)
    {
        if (!string.IsNullOrWhiteSpace(fileData))
        {
            _logger.LogInformation("Saving rules engine contents to cache.");
            await _cache.SetStringAsync(CommsCheckRuleEngineCacheKey, fileData);
            _logger.LogInformation("Saved rules engine contents to cache.");
        }
    }

    private async Task<string> LoadRulesContentFromFile(string fileName)
    {
        _logger.LogInformation("Loading rules engine contents from {fileLocation}", fileName);
        var data = await File.ReadAllTextAsync(fileName);
        _logger.LogInformation("Loaded rules engine contents from {fileLocation}", fileName);
        return data;
    }

    private async Task<RulesEngineAndHash> LoadRulesEngine(string fileName)
    {
        var fileData = await LoadData(fileName);
        var rules = LoadRulesEngineFromContext(fileData);
        var hash = GetHash(fileData);
        return new RulesEngineAndHash(rules, hash);
    }

    private string GetHash(string data)
    {
        var b = UTF8Encoding.UTF8.GetBytes(data);
        var hash = SHA256.HashData(b);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    private RulesEngine LoadRulesEngineFromContext(string data)
    {
        var workflow = System.Text.Json.JsonSerializer.Deserialize<List<Workflow>>(data);
        if (workflow == null)
            throw new SerializationException($"Workflow failed to deserialize from {_options.Value.JsonPath}");
        var rulesEngine = new RulesEngine(workflow.ToArray());
        return rulesEngine;
    }
}
