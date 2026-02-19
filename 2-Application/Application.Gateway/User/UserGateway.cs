using Application.Gateway.User.Models;
using Application.Services.ReqRes;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Core;
using Infra.Core.Authorization;
using Infra.Core.LogTrace;

namespace Application.Gateway.User;

[ServiceLocate(typeof(IUserGateway))]
public class UserGateway(IEventBus eventBus, IServiceProvider serviceProvider) : IUserGateway
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [LogTrace(returnType: typeof(AssignAppResponse))]
    public async Task<AssignAppResponse> AssignApp(AssignAppRequest request, CancellationToken token)
    {
        var addUserClaimequest = new AddUserClaimRequest(request.UserId, ClaimTypesEx.AppsAuthenticated, request.AppName);
        var response = await eventBus.Send<AddUserClaimRequest, AddUserClaimResponse>(addUserClaimequest, token).ConfigureAwait(false);

        return new AssignAppResponse(response.ErrorMessage, response.Success);
    }

    [LogTrace(returnType: typeof(RegisterUserResponse))]
    public async Task<RegisterUserResponse> Register(RegisterUserRawRequest request, CancellationToken token)
    {
        var pwdExtracted = PasswordHelper.ExtractPwdWithTimeVerification(request.rawPassword) ?? string.Empty; 
            
        var registerUserRequest = new RegisterUserRequest(
            request.UserName,
            pwdExtracted,
            request.DisplayName);

        return await eventBus.Send<RegisterUserRequest, RegisterUserResponse>(registerUserRequest, token);
    }
}