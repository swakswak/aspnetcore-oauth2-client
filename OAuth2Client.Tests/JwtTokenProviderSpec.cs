using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OAuth2Client.Security;

namespace OAuth2Client.Tests;

public class JwtTokenProviderSpec
{
    private ITokenProvider TokenProvider => TestServiceFactory.JwtTokenProvider;
    private Claim NameClaim => new(JwtRegisteredClaimNames.Name, "test-name");
    private IEnumerable<Claim> Claims => new[] { NameClaim };


    [Fact]
    public void Should_Write_JwtTokenString()
    {
        var jwtString = TokenProvider.Write(Claims);

        Assert.NotNull(jwtString);
        Assert.IsType<string>(jwtString);
    }

    [Fact]
    public void Should_Return_ValidatedPrincipal()
    {
        var jwtString = TokenProvider.Write(Claims);
        var claimsPrincipal = TokenProvider.Validate(jwtString);
        Assert.IsType<ClaimsPrincipal>(claimsPrincipal);
        Assert.True(claimsPrincipal.Claims.Any());

        var first = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
        Assert.NotNull(first);
        Assert.True(first?.Value == NameClaim.Value);
    }
}