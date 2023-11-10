using System.Security.Cryptography;
using System.Web;

public class CommsCheckItemSha
{
    private readonly HMACSHA256 alg;
    private readonly ILogger<CommsCheckItemSha> _logger;
    private readonly SemaphoreSlim _slim;
    public CommsCheckItemSha(ILogger<CommsCheckItemSha> logger, byte[] key)
    {
        _logger = logger;
        var b = new byte[key.Length];
        Array.Copy(key, b, key.Length);
        b[0] = (byte)1;
        alg = new HMACSHA256(b);
        //async object pool this instead of semslim. 
        _slim = new SemaphoreSlim(1, 1);
    }

    public async Task<string> GetSha(CommsCheckItem item, string reasonForHash)
    {
        await _slim.WaitAsync();
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
        finally
        {
            _slim.Release();
        }
    }
}