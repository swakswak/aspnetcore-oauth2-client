using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace OAuth2Client.Authentication.OAuth;

public class CustomOAuthOptions : IConfigureNamedOptions<OAuthOptions>
{
    public string ClaimsIssuer { get; set; } = null!;
    public string ClientId { get; set; } = null!; 
    public string ClientSecret { get; set; } = null!;
    public string CallbackPath { get; set; } = null!;
    public string AuthorizationEndpoint { get; set; } = null!;
    public string TokenEndpoint { get; set; } = null!;
    public string UserInformationEndpoint { get; set; } = null!;
    public bool SaveTokens { get; set; }
    public List<string> Scopes { get; set; } = null!;

    public static string AuthenticationScheme => "Custom";

    public void Configure(OAuthOptions options)
    {
        Configure(ClaimsIssuer, options);
    }

    public void Configure(string name, OAuthOptions options)
    {
        options.ClaimsIssuer = ClaimsIssuer;
        options.ClientId = ClientId;
        options.ClientSecret = ClientSecret;
        options.CallbackPath = CallbackPath;
        options.AuthorizationEndpoint = AuthorizationEndpoint;
        options.TokenEndpoint = TokenEndpoint;
        options.UserInformationEndpoint = UserInformationEndpoint;
        options.SaveTokens = SaveTokens;
        Scopes.ForEach(scope => options.ClaimActions.MapJsonKey(scope, scope));
        options.CorrelationCookie.SameSite = SameSiteMode.Strict;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
    }

    public override string ToString()
    {
        return
            $"{nameof(ClaimsIssuer)}: {ClaimsIssuer}, {nameof(ClientId)}: {ClientId}, {nameof(ClientSecret)}: {ClientSecret}, {nameof(CallbackPath)}: {CallbackPath}, {nameof(AuthorizationEndpoint)}: {AuthorizationEndpoint}, {nameof(TokenEndpoint)}: {TokenEndpoint}, {nameof(UserInformationEndpoint)}: {UserInformationEndpoint}, {nameof(SaveTokens)}: {SaveTokens}, {nameof(Scopes)}: {Scopes}";
    }
}