namespace OAuth2Client.Security.Jwt;

public class CustomJwtOptions
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public long ExpirationMinutes { get; set; }
    public string TokenName { get; set; } = null!;
}