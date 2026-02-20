using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record AddUserClaimRequest(string UserId, string ClaimType, string ClaimValue) : AppRequest, IRequest;

    public record AddUserClaimResponse(string ErrorMessage, bool Success) : AppResponse(ErrorMessage, Success), IResponse;
}
