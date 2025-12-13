using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record GetAllUsersQueryRequest() : IRequest;

    public record GetAllUsersQueryResponse(string Message, bool Success, List<Domain.User.Entities.User> Users): IResponse;
}
