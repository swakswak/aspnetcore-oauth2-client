using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace OAuth2Client.Authentication.Cookie;

public class CustomTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
{
    public CustomTicketDataFormat(ITokenProvider tokenProvider)
    {
        TokenProvider = tokenProvider;
    }

    private ITokenProvider TokenProvider { get; }

    public string Protect(AuthenticationTicket data)
    {
        return Protect(data, null);
    }

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        return TokenProvider.Write(data.Principal.Claims);
    }

    public AuthenticationTicket? Unprotect(string? protectedText)
    {
        return Unprotect(protectedText, null);
    }

    public AuthenticationTicket? Unprotect(string? protectedText, string? purpose)
    {
        if (protectedText is null) throw new AuthenticationException();
        var validatedClaimsPrincipal = TokenProvider.Validate(protectedText);

        return new AuthenticationTicket(
            NewClaimsPrincipal(validatedClaimsPrincipal.Claims),
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }

    private static ClaimsPrincipal NewClaimsPrincipal(IEnumerable<Claim> validatedClaims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(validatedClaims));
    }
}