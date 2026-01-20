using ArxOne.MrAdvice.Advice;
using Common.Core.AOP;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Core.Authorization
{
    public class ActiAuthorizeAttribute : Attribute, IMethodAsyncAdvice
    {
        private string _role;
        private string _right;
        public ActiAuthorizeAttribute(string Role, string Right)
        {
            _role = Role;
            _right = Right;
        }

        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var serviceProvider = context.GetMemberServiceProvider();
            var requestTraceService = serviceProvider?.GetRequiredService<IRequestTraceService>();


        }
    }
}
