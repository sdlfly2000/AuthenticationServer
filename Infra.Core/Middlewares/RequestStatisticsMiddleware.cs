using Common.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infra.Core.Middlewares
{
    [ServiceLocate(default)]
    public class RequestStatisticsMiddleware : IMiddleware
    {
        private readonly ILogger<RequestStatisticsMiddleware> _logger;

        private static ConcurrentDictionary<string, int> _requestStatitics = new ConcurrentDictionary<string, int>();

        public RequestStatisticsMiddleware(ILogger<RequestStatisticsMiddleware> logger)
        {
            _logger = logger;
        }

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _requestStatitics.AddOrUpdate(context.Request.Path, 1, (key, oldValue) => oldValue + 1);

            await next.Invoke(context);

            if (_requestStatitics.TryGetValue(context.Request.Path, out var reqNumberAfter))
            {
                _logger.LogInformation("RequestStatistics: {RequestPath}, {Count}", context.Request.Path, reqNumberAfter);
            }
        }
    }
}
