namespace OAuth2Client.Authentication.OAuth;

public class CustomOAuthClientOptions
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public List<string> Scopes { get; set; } = null!;
    public bool SaveTokens { get; set; }
}