using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OAuth2Client.Authentication;
using OAuth2Client.Authentication.Jwt;

namespace OAuth2Client.Tests;

internal static class TestServiceFactory
{
    public static ITokenProvider JwtTokenProvider =>
        new JwtTokenProvider(GetLoggerMock<JwtTokenProvider>(), Options.Create(CustomJwtOptions));
    
    private static CustomJwtOptions CustomJwtOptions =>
        new()
        {
            Issuer = "Issuer",
            Key = "my-jwt-token-secret-key-example",
            ExpirationMinutes = 30,
            Audience = "MyServiceUser"
        };
    
    private static ILogger<T> GetLoggerMock<T>()
    {
        var mock = new Mock<ILogger<T>>();
        return mock.Object;
    }
}