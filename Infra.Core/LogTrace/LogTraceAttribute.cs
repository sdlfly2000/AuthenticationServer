using ArxOne.MrAdvice.Advice;
using System.Diagnostics;

namespace Infra.Core.LogTrace
{
    public class LogTraceAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            LogTraceService.LogInformation($"Executing {context.TargetName}");

            await context.ProceedAsync();

            LogTraceService.LogInformation($"Executed {context.TargetName}, Time elapsed: {stopWatch.ElapsedMilliseconds} ms.");
        }
    }
}
