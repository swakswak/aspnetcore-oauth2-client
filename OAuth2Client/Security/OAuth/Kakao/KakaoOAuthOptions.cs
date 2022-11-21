using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace OAuth2Client.Security.OAuth.Kakao;

public class KakaoOAuthOptions : IConfigureNamedOptions<OAuthOptions>
{
    public KakaoOAuthOptions(IOptionsSnapshot<CustomOAuthClientOptions> oAuthClientOptionsSnapshot)
    {
        KakaoOAuthClientOptions = oAuthClientOptionsSnapshot.Get(KakaoOAuthDefaults.AuthenticationScheme);
    }

    private CustomOAuthClientOptions KakaoOAuthClientOptions { get; }

    public void Configure(OAuthOptions options)
    {
        Configure(KakaoOAuthDefaults.AuthenticationScheme, options);
    }

    public void Configure(string name, OAuthOptions options)
    {
        options.ClaimsIssuer = KakaoOAuthDefaults.ClaimIssuer;
        options.ClientId = KakaoOAuthClientOptions.ClientId;
        options.ClientSecret = KakaoOAuthClientOptions.ClientSecret;
        options.CallbackPath = KakaoOAuthDefaults.CallbackPath;
        options.AuthorizationEndpoint = KakaoOAuthDefaults.AuthorizationEndpoint;
        options.TokenEndpoint = KakaoOAuthDefaults.TokenEndpoint;
        options.UserInformationEndpoint = KakaoOAuthDefaults.UserInformationEndpoint;
        options.SaveTokens = KakaoOAuthClientOptions.SaveTokens;
        KakaoOAuthClientOptions.Scopes.ForEach(scope =>
            options.ClaimActions.MapJsonKey(scope, scope));
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;

        options.Events.OnCreatingTicket = async context =>
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            requestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(
                requestMessage, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            var kakaoUser = JsonSerializer.Deserialize<KakaoUser>(contentString);
            context.Identity?.AddClaims(
                new[]
                {
                    new Claim(ClaimTypes.Role, RoleName.User),
                    new Claim(JwtRegisteredClaimNames.Name, kakaoUser!.Properties.Nickname),
                    new Claim(JwtRegisteredClaimNames.Email, kakaoUser.KakaoAccount.Email!),
                    new Claim("profileImage", kakaoUser.Properties.ProfileImage)
                }
            );
            var parsed = JsonDocument.Parse(contentString);

            context.RunClaimActions(parsed.RootElement);
        };

        options.Events.OnTicketReceived = context =>
        {
            context.ReturnUri = context.ReturnUri == "/auth/challenge" ? "/" : context.ReturnUri;
            return Task.CompletedTask;
        };
    }
}