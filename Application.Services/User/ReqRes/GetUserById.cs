using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record GetUserByIdRequest(string UserId) : IRequest;

    public record GetUserByIdResponse(string Message, bool Success, Domain.User.Entities.User User): IResponse;
}
