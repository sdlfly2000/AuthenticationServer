using Application.Gateway.User.Models;
using Application.Services.ReqRes;
using Common.Core.AOP.LogTrace;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Core;
using Infra.Core.Authorization;
using System.Security.Claims;

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

    [LogTrace(returnType: typeof(AssignRoleResponse))]
    public async Task<AssignRoleResponse> AssignRole(AssignRoleRequest request, CancellationToken token)
    {
        var getUserByIdRequest = new GetUserByIdRequest(request.UserId);
        var user = (await eventBus
                        .Send<GetUserByIdRequest, GetUserByIdResponse>(getUserByIdRequest, token)
                        .ConfigureAwait(false))
                        .User;

        if (!user.Claims.Any(c => 
                c.Name.Equals(ClaimTypesEx.AppsAuthenticated) && 
                c.Value.Equals(request.AppName)))
        {
            throw new InvalidOperationException($"User:{user.UserName} is not authorized to app {request.AppName}.");
        }

        var roleClaimValue = $"{request.AppName}:{request.RoleName}";
        var addUserClaimRequest = new AddUserClaimRequest(request.UserId, ClaimTypes.Role, roleClaimValue);
        var response = await eventBus.Send<AddUserClaimRequest, AddUserClaimResponse>(addUserClaimRequest, token).ConfigureAwait(false);

        return new AssignRoleResponse(response.ErrorMessage, response.Success);
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