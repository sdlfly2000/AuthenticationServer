using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record AddUserClaimRequest(string UserId, string ClaimType, string ClaimValue) : IRequest;

    public record AddUserClaimResponse(string Message, bool Success) : IResponse;
}
