using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Infra.Core;
using Infra.Core.LogTrace;

namespace Application.Services.User.Commands
{
    [ServiceLocate(typeof(IRequestHandler<RegisterUserRequest, RegisterUserResponse>))]
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IUserPersistor _userPersistor;
        private readonly IServiceProvider _serviceProvider;

        public RegisterUserCommandHandler(IUserPersistor userPersistor, IServiceProvider serviceProvider)
        {
            _userPersistor = userPersistor;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
        {
            var domainResult = await _userPersistor.Add(new Domain.User.Entities.User(request.UserName)
            {
                DisplayName = request.DisplayName,
                PasswordHash = PasswordHelper.EncryptoPassword(request.Password)
            });

            return new RegisterUserResponse(domainResult.Message, domainResult.Success);
        }
    }
}
