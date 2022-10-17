using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2Client.Security.OAuth;

namespace OAuth2Client.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    public AuthController(ILogger<AuthController> logger)
    {
        Logger = logger;
    }

    private ILogger Logger { get; }

    [HttpGet]
    [Route("challenge")]
    public async Task ChallengeAsync()
    {
        Logger.LogInformation("[ChallengeAsync]");
        await HttpContext.ChallengeAsync(KakaoOAuthDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("logout")]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Response.Redirect(Url.Content("~/"));
    }
}