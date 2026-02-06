using Application.Services.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.Authorizations.Repositories;
using Infra.Core.LogTrace;
using Infra.Core.RequestTrace;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<AuthorizeRequest, AuthorizeResponse>))]
    public class GetAuthorizeRequestHandler : IRequestHandler<AuthorizeRequest, AuthorizeResponse>
    {
        // Keep the IServiceProvider which is used in AOP
        private readonly IServiceProvider _serviceProvider;
        private readonly IRoleRepository _roleRepository;
        private readonly IRequestContext _requestContext;

        public GetAuthorizeRequestHandler(IRoleRepository roleRepository, IRequestContext requestContext, IServiceProvider serviceProvider)
        {
            _roleRepository = roleRepository;
            _requestContext = requestContext;
            _serviceProvider = serviceProvider;
        }

        [LogTrace(returnType: typeof(AuthorizeResponse))]
        public async Task<AuthorizeResponse> Handle(AuthorizeRequest request, CancellationToken cancellationToken)
        {
            var roleName = _requestContext.CurrentUserRole;
            var role = await _roleRepository.GetByRoleName(roleName, cancellationToken).ConfigureAwait(false);
            foreach (string right in request.Rights)
            {
                if (!role.HasRight(right))
                {
                    return new AuthorizeResponse($"Role:{role.RoleName} does not authorize right:{right}.", true, false);
                }
            }
            return new AuthorizeResponse("Authorized.", true, true);
        }
    }
}
