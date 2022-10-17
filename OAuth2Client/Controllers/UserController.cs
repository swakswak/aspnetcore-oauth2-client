using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2Client.Dtos;

namespace OAuth2Client.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    public UserController(ILogger<UserController> logger)
    {
        Logger = logger;
    }

    private ILogger<UserController> Logger { get; }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "User")]
    public UserDto Me()
    {
        var userClaims = HttpContext.User.Claims;
        Logger.LogInformation("{}", userClaims);
        return UserDto.Of(userClaims);
    }
}