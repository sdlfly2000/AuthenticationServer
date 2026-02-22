using Common.Core.CQRS.Request;
using System.Security.Claims;
using Common.Core.DependencyInjection;
using Infra.Core.Authorization;
using Application.Services.ReqRes;
using Common.Core.AOP.LogTrace;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetClaimTypesRequest, GetClaimTypesResponse>))]
    public class GetAllClaimTypesRequestHandler : IRequestHandler<GetClaimTypesRequest, GetClaimTypesResponse>
    {
        // Keep the IServiceProvider which is used in AOP
        private readonly IServiceProvider _serviceProvider;

        public GetAllClaimTypesRequestHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [LogTrace(returnType: typeof(GetClaimTypesResponse))]
        public async Task<GetClaimTypesResponse> Handle(GetClaimTypesRequest request, CancellationToken cancellationToken)
        {
            var claimTypeValues = typeof(ClaimTypes).GetFields()
                .Where(type => type.IsPublic && type.IsStatic)
                .Where(type => !type.Name.Equals("Role"))
                .Select(type => new ClaimTypeValues(type.Name, type.GetValue(type.Name)?.ToString()))
                .ToList();

            claimTypeValues.AddRange(
                typeof(ClaimTypesEx).GetFields()
                .Where(type => type.IsPublic && type.IsStatic)
                .Where(type => !type.Name.Equals(nameof(ClaimTypesEx.AppsAuthenticated)))
                .Select(type => new ClaimTypeValues(type.Name, type.GetValue(type.Name)?.ToString()))
                .ToList());

            return await Task.FromResult(new GetClaimTypesResponse(string.Empty, true, claimTypeValues));
        }
    }
}
