using Application.Services.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Infra.Core;
using Infra.Core.LogTrace;
using System.Security.Claims;

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

        [LogTrace(returnType: typeof(RegisterUserResponse))]
        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var newUser = new Domain.User.Entities.User(request.UserName)
            {
                DisplayName = request.DisplayName,
                PasswordHash = PasswordHelper.EncryptoPassword(request.Password)
            };

            newUser.AddClaim(ClaimTypes.NameIdentifier, newUser.Id.Code, true);
            
            var domainResult = await _userPersistor.Add(newUser);

            return new RegisterUserResponse(domainResult.Message, domainResult.Success);
        }
    }
}
