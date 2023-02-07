using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuth2Client.Controllers;

[ApiController]
[Route("resources")]
public class ResourceController : ControllerBase
{
    [HttpGet]
    [Authorize(
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
        Policy = PolicyName.ResourcePolicy
    )]
    public object GetResource([FromHeader] string detail)
    {
        return new {Color = detail};
    }
}