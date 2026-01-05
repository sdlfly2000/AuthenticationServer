using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.ReqRes
{
    public record GetUserByIdRequest(string userId) : AppRequest, IRequest;

    public record GetUserByIdResponse: AppResponse, IResponse
    {
        public GetUserByIdResponse(string Message, bool Success) : base(Message, Success)
        {
        }

        public GetUserByIdResponse(string Message, bool Success, Domain.User.Entities.User? user) : base(Message, Success)
        {
            User = user;
        }

        public Domain.User.Entities.User? User { get; private set; }

    };
}
