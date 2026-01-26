using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record DeleteUserClaimRequest(string UserId, string ClaimType, string ClaimValue) : AppRequest, IRequest;

    public record DeleteUserClaimResponse : AppResponse, IResponse 
    {
        public DeleteUserClaimResponse(string Message, bool Success) : base(Message, Success)
        {
            
        }
    };
}
