using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record UpdateUserClaimRequest(string UserId, string ClaimType, string ClaimValue) : IRequest;

    public record UpdateUserClaimResponse(string Message, bool Success) : IResponse;
}
