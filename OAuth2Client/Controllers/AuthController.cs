using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OAuth2Client.Security.OAuth;
using OAuth2Client.Security.OAuth.Kakao;

namespace OAuth2Client.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    public AuthController(ILogger<AuthController> logger) => Logger = logger;

    private ILogger Logger { get; }

    [HttpGet("challenge")]
    public async Task ChallengeAsync()
    {
        Logger.LogInformation("[ChallengeAsync]");
        await HttpContext.ChallengeAsync(KakaoOAuthDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleName.User)]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Response.Redirect(Url.Content("~/"));
    }
}