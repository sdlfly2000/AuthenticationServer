using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record RegisterUserRequest(string UserName, string Password, string DisplayName): IRequest;

    public record RegisterUserResponse(string Message, bool Success) : IResponse;
}
