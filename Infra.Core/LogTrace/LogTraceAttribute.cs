using ArxOne.MrAdvice.Advice;
using Infra.Core.RequestTrace;
using Serilog;
using System.Diagnostics;

namespace Infra.Core.LogTrace
{
    public class LogTraceAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            Log.Information($"New Trace Id: {{TraceId}}, Executing {context.TargetName}", RequestTraceScoped.TraceId);

            await context.ProceedAsync();

            Log.Information($"New Trace Id: {{TraceId}}, Time elapsed: {stopWatch.ElapsedMilliseconds} ms.", RequestTraceScoped.TraceId);
        }
    }
}
