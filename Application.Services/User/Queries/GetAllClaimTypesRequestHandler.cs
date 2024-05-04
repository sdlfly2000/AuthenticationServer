using Common.Core.CQRS.Request;
using Application.Services.User.ReqRes;
using System.Security.Claims;
using Common.Core.DependencyInjection;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetClaimTypesRequest, GetClaimTypesResponse>))]
    public class GetAllClaimTypesRequestHandler : IRequestHandler<GetClaimTypesRequest, GetClaimTypesResponse>
    {
        public async Task<GetClaimTypesResponse> Handle(GetClaimTypesRequest request)
        {
            var claimTypeValues = typeof(ClaimTypes).GetFields().Where(type => type.IsPublic && type.IsStatic)
                .Select(type => new ClaimTypeValues(type.Name, type.GetValue(type.Name)?.ToString()))
                .ToList();

            return await Task.FromResult(new GetClaimTypesResponse(string.Empty, true, claimTypeValues));
        }
    }
}
