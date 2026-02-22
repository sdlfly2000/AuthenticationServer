using ArxOne.MrAdvice.Advice;
using Common.Core.AOP;
using Common.Core.AOP.LogTrace;
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
        var requestContext = serviceProvider?.GetRequiredService<IRequestContext>();
        var roleRepository = serviceProvider?.GetRequiredService<IRoleRepository>();
        var currentUserRoles = requestContext?.CurrentUserRoles;
        var logger = serviceProvider?.GetRequiredService<ILogger>();

        ArgumentNullException.ThrowIfNull(requestContext, $"{nameof(RequestContext)} is null");
        ArgumentNullException.ThrowIfNull(roleRepository, $"{nameof(IRoleRepository)} is null");
        ArgumentNullException.ThrowIfNull(currentUserRoles, $"{nameof(currentUserRoles)} in {nameof(RequestContext)} is null");
        ArgumentNullException.ThrowIfNull(logger, $"{nameof(ILogger)} is null");

        var roles = await roleRepository.GetByRoleNames(currentUserRoles, cancellationToken).ConfigureAwait(false);
        var rights = roles.SelectMany(r => r.Rights.Select(r => r.RightName)).ToList();

        if (!rights.Contains(_right))
        {
            logger.Warning($"Trace Id: {{TraceId}}, Not Authorized to operate via Right:{_right}.", requestContext?.TraceId);
            UnauthorizedException.Throw(_right);
        }
        
        await context.ProceedAsync().ConfigureAwait(false);
    }
}
