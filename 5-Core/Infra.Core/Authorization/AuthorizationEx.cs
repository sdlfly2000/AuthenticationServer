using Infra.Core.Configurations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Infra.Core.Authorization;

public static class AuthorizationEx
{
    public static bool VerifyAppName(AuthorizationHandlerContext context)
    {
        var configuration = ConfigurationService.GetConfiguration();
        var appName = configuration["Application:Properties:Name"] ?? string.Empty;

        var appClaims = context.User.Claims
            .Where(c => c.Type.Equals(ClaimTypesEx.AppsAuthenticated))
            .Select(claim => claim.Value)
            .ToList();

        return appClaims.Contains(appName);
    }

    public static bool VerifyAdminRole(AuthorizationHandlerContext context)
    {
        var configuration = ConfigurationService.GetConfiguration();
        var appName = configuration["Application:Properties:Name"];

        var roleClaim = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
        return roleClaim?.Split(',').ToList().Contains($"{appName}:Admin") ?? false;
    }
}
