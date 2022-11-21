using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using OAuth2Client.Security.Jwt;
using OAuth2Client.Security.OAuth;

namespace OAuth2Client.Security.Cookie;

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
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = true;

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    }
}