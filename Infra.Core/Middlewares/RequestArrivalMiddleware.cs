using Common.Core.DependencyInjection;
using Infra.Core.RequestTrace;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infra.Core.Middlewares
{
    [ServiceLocate(default)]
    public class RequestArrivalMiddleware : IMiddleware
    {
        private readonly ILogger<RequestArrivalMiddleware> _logger;
        private readonly IRequestTraceService _requestTraceService;
        private readonly IServiceProvider _serviceProvider;

        private static ConcurrentDictionary<string, int> _requestStatitics = new ConcurrentDictionary<string, int>();

        public RequestArrivalMiddleware(
            ILogger<RequestArrivalMiddleware> logger,
            IRequestTraceService requestTraceService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _requestTraceService = requestTraceService;
            _serviceProvider = serviceProvider;
        }

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _requestStatitics.AddOrUpdate(context.Request.Path, 1, (key, oldValue) => oldValue + 1);

            _requestTraceService.TraceId = Guid.NewGuid().ToString();

            var requestTraceScoped = new RequestContext();

            RequestContext.ServiceProvider = _serviceProvider;

            await next.Invoke(context);

            if (_requestStatitics.TryGetValue(context.Request.Path, out var reqNumberAfter))
            {
                _logger.LogInformation("RequestStatistics: {RequestPath}, {Count}", context.Request.Path, reqNumberAfter);
            }

            requestTraceScoped.Dispose();
        }
    }
}
