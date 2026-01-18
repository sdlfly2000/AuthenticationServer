using Infra.Core.Configurations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Infra.Core.Authorization;

public static class AuthorizationEx
{
    public static bool VerifyAppName(AuthorizationHandlerContext context)
    {
        var configuration = ConfigurationService.GetConfiguration();
        var appName = configuration["Application:Properties:Name"];

        var appClaim = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypesEx.AppsAuthencated))?.Value;

        return appClaim?.Split(',').ToList().Contains(appName) ?? false;
    }

    public static bool VerifyAdminRole(AuthorizationHandlerContext context)
    {
        var configuration = ConfigurationService.GetConfiguration();
        var appName = configuration["Application:Properties:Name"];

        var roleClaim = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
        return roleClaim?.Split(',').ToList().Contains($"{appName}:Admin") ?? false;
    }
}
