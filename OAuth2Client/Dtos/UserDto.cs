using System.Security.Claims;

namespace OAuth2Client.Dtos;

public record UserDto
(
    string? Nickname,
    string? Email
)
{
    public static UserDto Of(IEnumerable<Claim> claims)
    {
        var dictionary = claims.ToDictionary(c => c.Type, c => c.Value);
        return new UserDto(
            dictionary[ClaimTypes.Name],
            dictionary[ClaimTypes.Email]
        );
    }
}