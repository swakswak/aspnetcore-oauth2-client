using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Moq;
using OAuth2Client.Security;
using OAuth2Client.Security.OAuth;

namespace OAuth2Client.Tests;

public class CustomOAuthStateDataFormatSpec
{
    private CustomOAuthStateDataFormat StateDataFormat = new CustomOAuthStateDataFormat(Options.Create(TestAesOptions()));
    private static AesOptions TestAesOptions()
    {
        var aesOptions = new AesOptions
        {
            Password = "my-secret-password"
        };

        return aesOptions;
    }

    [Fact]
    public void Should_Encrypt()
    {
        var mockAuthenticationProperties = new Mock<AuthenticationProperties>();
        var protectedText = StateDataFormat.Protect(mockAuthenticationProperties.Object);
        
        Assert.NotNull(protectedText);
    }

    [Fact]
    public void Should_Decrypt()
    {
        var mockAuthenticationProperties = new Mock<AuthenticationProperties>();
        var protectedText = StateDataFormat.Protect(mockAuthenticationProperties.Object);
        
        Assert.NotNull(protectedText);

        var authenticationProperties = StateDataFormat.Unprotect(protectedText);
        Assert.NotNull(authenticationProperties);
    }
}