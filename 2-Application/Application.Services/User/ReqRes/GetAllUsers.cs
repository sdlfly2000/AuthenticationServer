using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.ReqRes
{
    public record GetAllUsersQueryRequest() : AppRequest, IRequest;

    public record GetAllUsersQueryResponse : AppResponse, IResponse
    {
        public GetAllUsersQueryResponse(string Message, bool Success) : base(Message, Success)
        {
            Users = new List<Domain.User.Entities.User>();
        }

        public GetAllUsersQueryResponse(string Message, bool Success, List<Domain.User.Entities.User> UsersInput) : base(Message, Success)
        {
            Users = UsersInput;
        }

        public List<Domain.User.Entities.User> Users { get; init; }
    };
}
