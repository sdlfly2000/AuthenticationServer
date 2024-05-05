using ArxOne.MrAdvice.Advice;
using Serilog;
using System.Diagnostics;

namespace Infra.Core.LogTrace
{
    public class LogTraceAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            Log.Information($"Executing {context.TargetName}");

            await context.ProceedAsync();

            Log.Information($"Executed {context.TargetName}, Time elapsed: {stopWatch.ElapsedMilliseconds} ms.");
        }
    }
}
