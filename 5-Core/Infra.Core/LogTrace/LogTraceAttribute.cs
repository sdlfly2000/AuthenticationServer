using ArxOne.MrAdvice.Advice;
using Infra.Core.Extentions;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics;

namespace Infra.Core.LogTrace
{
    public class LogTraceAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            var serviceProvider = context.GetMemberServiceProvider();

            var requestTraceService = serviceProvider?.GetRequiredService<IRequestTraceService>();

            var logger = serviceProvider?.GetRequiredService<ILogger>();

            logger?.Information($"Trace Id: {{TraceId}}, Executing {{MetricExecutionTarget}}", requestTraceService?.TraceId, context.Target);

            await context.ProceedAsync();

            logger?.Information($"Trace Id: {{TraceId}}, Executed {{MetricExecutionTargetSuccess}} in {{MetricExecutionTimeInMs}} ms.", requestTraceService?.TraceId, context.Target, stopWatch.ElapsedMilliseconds);
        }
    }


}
