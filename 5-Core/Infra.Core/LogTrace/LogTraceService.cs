using Common.Core.DependencyInjection;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Logging;

namespace Infra.Core.LogTrace
{
    [ServiceLocate(typeof(LogTraceService), ServiceType.Scoped)]
    public class LogTraceService
    {
        private static ILogger<LogTraceService> _logger;
        private static IRequestContext _requestContext;

        public LogTraceService(
            ILogger<LogTraceService> logger,
            IRequestContext requestContext)
        {
            _logger = logger;
            _requestContext = requestContext;
        }
        
        public static void LogInformation(string message)
        {
            _logger.LogInformation($"{message}, Trace Id: {{TraceId}}", _requestContext.TraceId);
        }
    }
}
