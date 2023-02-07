using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace OAuth2Client.Security.Policy;

public class ResourceAuthorizationHandler : IAuthorizationHandler, IAuthorizationRequirement
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.Claims.FirstOrDefault(claim => ClaimTypes.Role.Equals(claim.Type)) is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var permissionClaim = context.User.Claims.FirstOrDefault(claim => "permission".Equals(claim.Type));
        if (permissionClaim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var permission = permissionClaim!.Value;
        var split = permission.Split(":");

        if (split.Length < 2)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var resource = split[0];
        var detail = split[1];

        if ("Resource".Equals(resource))
        {
            if ("Blue".Equals(detail) || "Green".Equals(detail))
            {
                var contextResource = (DefaultHttpContext) context.Resource!;
                contextResource.Request.Headers["detail"] = detail;
                Console.WriteLine(contextResource);
                context.Succeed(this);
                return Task.CompletedTask;
            }
        }
        
        context.Fail();
        return Task.CompletedTask;
    }
}