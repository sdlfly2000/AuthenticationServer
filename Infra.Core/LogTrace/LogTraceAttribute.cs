using ArxOne.MrAdvice.Advice;
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

            var requestTraceService = RequestContext.ServiceProvider.GetRequiredService<IRequestTraceService>();

            Log.Information($"Trace Id: {{TraceId}}, Executing {context.TargetName}", requestTraceService.TraceId);

            await context.ProceedAsync();

            Log.Information($"Trace Id: {{TraceId}}, Time elapsed: {stopWatch.ElapsedMilliseconds} ms.", requestTraceService.TraceId);
        }
    }
}
