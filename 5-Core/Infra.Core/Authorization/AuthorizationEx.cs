using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Infra.Core.Authorization;

public static class AuthorizationEx
{
    public static bool VerifyAppName(AuthorizationHandlerContext context)
    {
        var configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();

        var appClaim = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypesEx.AppsAuthencated))?.Value;

        return appClaim?.Split(',').ToList().Contains(configuration["Application:Properties:Name"]) ?? false;
    }
}
