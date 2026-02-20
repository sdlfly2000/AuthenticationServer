using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record GetUserByIdRequest(string userId) : AppRequest, IRequest;

    public record GetUserByIdResponse(string Message, bool Success, Domain.User.Entities.User User) : AppResponse(Message, Success), IResponse;
}
