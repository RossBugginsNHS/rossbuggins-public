namespace CommsCheck;

using System.Text.Json.Serialization;

public readonly record struct PostalCode 
{
    public string PostCode {get;init;}

    public bool IsZZ99 => PostCode!=null && PostCode.StartsWith("ZZ99");

    [JsonConstructor]
    public PostalCode(string postCode)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(postCode, nameof(postCode));
        PostCode = ParsePostcodeString(postCode);
    }

    public static PostalCode Parse(string postCode)
    {
        return new PostalCode(postCode);
    }

    private static string ParsePostcodeString(string postcode)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(postcode, nameof(postcode));
        return postcode.ToUpper().Replace(" ", "");
    }

    public static PostalCode Empty =>  new PostalCode();

    public PostalCode DistrictOnly() => new PostalCode(string.Concat(PostCode.Take(4)));
}
