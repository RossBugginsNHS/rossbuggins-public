using System.Security.Cryptography;
using System.Web;

public class CommsCheckItemSha
{
    private readonly HMACSHA256 alg;
    private readonly ILogger<CommsCheckItemSha> _logger;
    public CommsCheckItemSha(ILogger<CommsCheckItemSha> logger, byte[] key)
    {
        _logger = logger;
        var b = new byte[key.Length];
        Array.Copy(key, b, key.Length);
        b[0] = (byte)1;
        alg = new HMACSHA256(b);
    }

    public string GetSha(CommsCheckItem item)
    {
        var str = item.ToString().Trim().ToUpper();
        var b = System.Text.Encoding.UTF8.GetBytes(str);
        var hash = alg.ComputeHash(b);
        var hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

        _logger.LogInformation("Hash of {id} for item {item} with bytes {b}", hashStr, str, b);
        return hashStr;
    }
}