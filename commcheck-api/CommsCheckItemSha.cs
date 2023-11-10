using System.Security.Cryptography;
using System.Web;

public class CommsCheckItemSha
{
    private readonly HMACSHA256 alg;
    public CommsCheckItemSha(byte[] key)
    {
        var b = new byte[key.Length];
        Array.Copy(key, b, key.Length);
        b[0] = (byte)1;
        alg = new HMACSHA256(b);
    }

    public string GetSha(CommsCheckItem item)
    {
        var str = item.ToString();
        var b = System.Text.Encoding.UTF8.GetBytes(str);
        var hash = alg.ComputeHash(b);
        var hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();
        return hashStr;
    }
}