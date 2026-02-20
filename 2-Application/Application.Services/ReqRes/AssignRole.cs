using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record AssignRoleRequest(string UserId, string AppName, string RoleName): AppRequest, IRequest;

    public record AssignRoleResponse(string Message, bool Success) : AppResponse(Message, Success), IResponse;
}
