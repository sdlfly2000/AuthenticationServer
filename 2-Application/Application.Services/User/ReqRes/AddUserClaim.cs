using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.ReqRes
{
    public record AddUserClaimRequest(string UserId, string ClaimType, string ClaimValue) : AppRequest, IRequest;

    public record AddUserClaimResponse: AppResponse, IResponse
    {
        public AddUserClaimResponse(string Message, bool Success) : base(Message, Success)
        {
        }
    }
}
