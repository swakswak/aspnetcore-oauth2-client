using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using OAuth2Client.Authentication.Jwt;
using OAuth2Client.Authentication.OAuth;

namespace OAuth2Client.Authentication.Cookie;

public class CustomCookieAuthOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    public CustomCookieAuthOptions(ISecureDataFormat<AuthenticationTicket> ticketDataFormat,
        IOptions<CustomJwtOptions> wrappedJwtOptions)
    {
        TicketDataFormat = ticketDataFormat;
        JwtOptions = wrappedJwtOptions.Value;
    }

    private ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; }
    private CustomJwtOptions JwtOptions { get; }

    public void Configure(CookieAuthenticationOptions options) =>
        Configure(CookieAuthenticationDefaults.AuthenticationScheme, options);

    public void Configure(string name, CookieAuthenticationOptions options)
    {
        options.TicketDataFormat = TicketDataFormat;
        options.Cookie.MaxAge = TimeSpan.FromMinutes(JwtOptions.ExpirationMinutes);
        options.Cookie.Name = JwtOptions.TokenName;

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        
        options.Events.OnRedirectToLogin = async context =>
        {
            // context.RedirectUri = "/weatherforecast";
            // context.Response.StatusCode = 401;
            // context.RedirectUri = "/auth/challenge";
            await context.HttpContext.ChallengeAsync(KakaoOAuthDefaults.AuthenticationScheme);
        };
    }
}