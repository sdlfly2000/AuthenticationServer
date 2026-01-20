using Common.Core.DependencyInjection;
using Infra.Core.Configurations;
using Infra.Core.RequestTrace;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infra.Core.Middlewares
{
    [ServiceLocate(default)]
    public class RequestArrivalMiddleware : IMiddleware
    {
        private readonly ILogger<RequestArrivalMiddleware> _logger;
        private readonly IRequestTraceService _requestTraceService;

        public RequestArrivalMiddleware(
            ILogger<RequestArrivalMiddleware> logger,
            IRequestTraceService requestTraceService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _requestTraceService = requestTraceService;
        }

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _requestTraceService.TraceId = Guid.NewGuid().ToString();
            var appName = ConfigurationService.GetConfiguration()["Application:Properties:Name"];

            var currentUserId = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var currentUserRoles = context.User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
            _requestTraceService.CurrentUserId = string.IsNullOrEmpty(currentUserId) ? Guid.Empty : Guid.Parse(currentUserId);
            _requestTraceService.CurrentUserRole = GetRoleFromClaim(appName, currentUserRoles);
                ;
            await next.Invoke(context);
        }

        private string GetRoleFromClaim(string? appName, string? userRoles)
        {
            if (string.IsNullOrEmpty(userRoles))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(appName))
            {
                return string.Empty;
            }

            var roles = new Dictionary<string, string>();
            userRoles.Split(',').ToList().ForEach(role =>
            {
                var rolePair = role.Split(':');
                roles.Add(rolePair[0], rolePair[1]);
            });

            return roles.TryGetValue(appName, out string? value) ? value : string.Empty;
        }
    }
}
