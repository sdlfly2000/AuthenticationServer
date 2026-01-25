using ArxOne.MrAdvice.Advice;
using Common.Core.AOP;
using Domain.Authorizations.Repositories;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Domain.Authorizations.Attributes;

public class ActiAuthorizeAttribute : Attribute, IMethodAsyncAdvice
{
    private string _role;
    private string _right;

    public ActiAuthorizeAttribute(string? Role, string? Right)
    {
        _role = Role ?? string.Empty;
        _right = Right ?? string.Empty;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var targetCancellationToken = context.Arguments.SingleOrDefault(a => a.GetType() == typeof(CancellationToken));

        var cancellationToken = targetCancellationToken != null
                                    ? (CancellationToken)targetCancellationToken
                                    : CancellationToken.None;

        var serviceProvider = context.GetMemberServiceProvider();
        var requestTraceService = serviceProvider?.GetRequiredService<IRequestTraceService>();
        var roleRepository = serviceProvider?.GetRequiredService<IRoleRepository>();
        var currentUserRole = requestTraceService?.CurrentUserRole;
        var logger = serviceProvider?.GetRequiredService<ILogger>();

        ArgumentNullException.ThrowIfNull(requestTraceService, $"{nameof(RequestTraceService)} is null");
        ArgumentNullException.ThrowIfNull(roleRepository, $"{nameof(IRoleRepository)} is null");
        ArgumentNullException.ThrowIfNull(currentUserRole, $"{nameof(currentUserRole)} in {nameof(RequestTraceService)} is null");
        ArgumentNullException.ThrowIfNull(logger, $"{nameof(ILogger)} is null");

        var currentRole = await roleRepository.GetByRoleName(currentUserRole, cancellationToken).ConfigureAwait(false);

        if ( !_role.Equals(currentUserRole) || !currentRole.HasRight(_right))
        {
            logger.Warning($"Trace Id: {{TraceId}}, Not Authorized to operate via Role:{_role} or Right:{_right} .", requestTraceService?.TraceId);
            return;
        }

        await context.ProceedAsync().ConfigureAwait(false);
    }
}
