using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace OAuth2Client.Authentication.Jwt;

public class JwtTokenProvider : ITokenProvider
{
    public JwtTokenProvider(ILogger<JwtTokenProvider> logger, IOptions<CustomJwtOptions> customJwtOptions)
    {
        Logger = logger;
        Options = customJwtOptions.Value;
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.SecretKey));
    }

    private ILogger Logger { get; }

    private CustomJwtOptions Options { get; }
    private SymmetricSecurityKey SecurityKey { get; }

    public string Write(IEnumerable<Claim> claims)
    {
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var claimList = claims.ToList();
        var securityToken = new JwtSecurityToken(
            Options.Issuer,
            Options.Audience,
            claimList,
            expires: DateTime.Now.AddMinutes(Options.ExpirationMinutes),
            signingCredentials: credentials
        );
        var handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(securityToken);
    }

    public ClaimsPrincipal Validate(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var claimsPrincipal = handler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                IssuerSigningKey = SecurityKey,
                ValidAudience = Options.Audience,
                ValidIssuer = Options.Issuer
            },
            out _
        );
        return claimsPrincipal;
    }
}