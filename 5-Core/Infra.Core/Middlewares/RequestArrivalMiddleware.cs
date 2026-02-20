using Common.Core.DependencyInjection;
using Infra.Core.Configurations;
using Infra.Core.RequestTrace;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infra.Core.Middlewares
{
    [ServiceLocate(null)]
    public class RequestArrivalMiddleware(
        IRequestContext requestContext)
        : IMiddleware
    {
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            requestContext.TraceId = Guid.NewGuid().ToString();
            var appName = ConfigurationService.GetConfiguration()["Application:Properties:Name"];

            var currentUserId = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var roles = context.User.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Role))
                .Select(c => c.Value)
                .ToList();
            requestContext.CurrentUserId = string.IsNullOrEmpty(currentUserId) ? Guid.Empty : Guid.Parse(currentUserId);
            requestContext.CurrentUserRoles = GetRoleFromClaims(appName, roles);

            await next.Invoke(context);
        }

        private List<string> GetRoleFromClaims(string? appName, List<string> allRoles)
        {
            var roles = new List<string>();
            
            if (allRoles.Count == 0 || string.IsNullOrEmpty(appName))
            {
                return roles;
            }

            allRoles.ForEach(role =>
            {
                var rolePair = role.Split(':');
                if(rolePair[0].Equals(appName, StringComparison.InvariantCultureIgnoreCase))
                    roles.Add(rolePair[1]);
            });

            return roles;
        }
    }
}
