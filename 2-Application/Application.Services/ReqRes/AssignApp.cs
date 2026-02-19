using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record AssignAppRequest(string UserId, string AppName): AppRequest, IRequest;

    public record AssignAppResponse(string Message, bool Success) : AppResponse(Message, Success), IResponse;
}
