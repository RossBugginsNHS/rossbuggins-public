using System.Security.Cryptography;
using Microsoft.Extensions.Options;

public class HashWrapper
{
    private readonly HMACSHA256 alg;
    private readonly ILogger<HashWrapper> _logger;

       public HashWrapper(ILogger<HashWrapper> logger, IOptions<HashWrapperOptions> options)
    {
        _logger = logger;
        var key = options.Value.HashKey;
        var b = new byte[key.Length];
        Array.Copy(key, b, key.Length);
        b[0] = (byte)1;
        alg = new HMACSHA256(b);
    }

    public async Task<string> GetSha(CommsCheckItem item, string reasonForHash)
    {
        try
        {
            var str = item.ToString().Trim().ToUpper();
            var b = System.Text.Encoding.UTF8.GetBytes(str);
            var hash = alg.ComputeHash(b);
            var hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

            _logger.LogInformation("Hash for {reasonForHash} of {id} for item {item}", reasonForHash, hashStr, str);
            return hashStr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failure in get sha.");
            throw;
        }
    }

}
