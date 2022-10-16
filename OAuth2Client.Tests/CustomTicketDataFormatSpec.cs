using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Moq;
using OAuth2Client.Authentication.Cookie;
using OAuth2Client.Authentication.OAuth;

namespace OAuth2Client.Tests;

public class CustomTicketDataFormatSpec
{
    private CustomTicketDataFormat TicketDateFormat => new(TestServiceFactory.JwtTokenProvider);

    [Fact]
    public void Should_Protect()
    {
        var protectedString = Protect();

        Assert.NotNull(protectedString);
    }

    [Fact]
    public void Should_Unprotect()
    {
        var unprotected = TicketDateFormat.Unprotect(Protect());
        
        Assert.NotNull(unprotected);
    }

    private string Protect()
    {
        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        var authenticationTicket = new Mock<AuthenticationTicket>(
            MockBehavior.Strict, claimsPrincipalMock.Object,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        return TicketDateFormat.Protect(authenticationTicket.Object);
    }
}