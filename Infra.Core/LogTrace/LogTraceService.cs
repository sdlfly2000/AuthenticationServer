using Common.Core.DependencyInjection;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Logging;

namespace Infra.Core.LogTrace
{
    [ServiceLocate(typeof(LogTraceService), ServiceType.Scoped)]
    public class LogTraceService
    {
        private static ILogger<LogTraceService> _logger;
        private static IRequestTraceService _requestTraceService;

        public LogTraceService(
            ILogger<LogTraceService> logger,
            IRequestTraceService requestTraceService)
        {
            _logger = logger;
            _requestTraceService = requestTraceService;
        }
        
        public static void LogInformation(string message)
        {
            _logger.LogInformation($"{message}, Trace Id: {{TraceId}}", _requestTraceService.TraceId);
        }
    }
}
