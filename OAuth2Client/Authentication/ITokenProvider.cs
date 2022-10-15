using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OAuth2Client.Authentication;

public interface ITokenProvider
{
    string Write(IEnumerable<Claim> claims);
    ClaimsPrincipal Validate(string token);
}