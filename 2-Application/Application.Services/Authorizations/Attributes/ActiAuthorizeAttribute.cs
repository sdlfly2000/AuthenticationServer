using ArxOne.MrAdvice.Advice;
using Common.Core.AOP;
using Domain.Authorizations.Enum;
using Domain.Authorizations.Repositories;
using Infra.Core.Exceptions;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Services.Authorizations.Attributes;

public class ActiAuthorizeAttribute : Attribute, IMethodAsyncAdvice
{
    private string _right;

    public ActiAuthorizeAttribute(EnumRights Right)
    {
        _right = Right.ToString();
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

        if (!string.IsNullOrEmpty(_right) && !currentRole.HasRight(_right))
        {
            logger.Warning($"Trace Id: {{TraceId}}, Not Authorized to operate via Right:{_right} .", requestTraceService?.TraceId);
            UnauthorizedException.Throw(_right);
        }

        await context.ProceedAsync().ConfigureAwait(false);
    }
}
