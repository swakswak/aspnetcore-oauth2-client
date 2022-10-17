using System.Security.Claims;

namespace OAuth2Client.Security;

public interface ITokenProvider
{
    string Write(IEnumerable<Claim> claims);
    ClaimsPrincipal Validate(string token);
}