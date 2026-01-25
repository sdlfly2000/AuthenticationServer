using Application.Gateway.User.Models;
using Application.Services.User.ReqRes;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Core;
using Infra.Core.LogTrace;



namespace Application.Gateway.User
{
    [ServiceLocate(typeof(IUserGateway))]
    public class UserGateway : IUserGateway
    {
        private readonly IEventBus _eventBus;
        private readonly IServiceProvider _serviceProvider;

        public UserGateway(IEventBus eventBus, IServiceProvider serviceProvider)
        {
            _eventBus = eventBus;
            _serviceProvider = serviceProvider;
        }

        [LogTrace(returnType: typeof(RegisterUserResponse))]
        public async Task<RegisterUserResponse> Register(RegisterUserRawRequest request, CancellationToken token)
        {
            var pwdExtracted = PasswordHelper.ExtractPwdWithTimeVerification(request.rawPassword) ?? string.Empty; 
            
            var registerUserRequest = new RegisterUserRequest(
                request.UserName,
                pwdExtracted,
                request.DisplayName);

            return await _eventBus.Send<RegisterUserRequest, RegisterUserResponse>(registerUserRequest, token);
        }
    }
}
