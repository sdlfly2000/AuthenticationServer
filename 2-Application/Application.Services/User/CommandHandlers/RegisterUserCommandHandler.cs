using Application.Services.Events.Messages;
using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Infra.Core;
using Infra.Core.LogTrace;
using Infra.Core.MessageQueue.RabbitMQ.Services;

namespace Application.Services.User.Commands
{
    [ServiceLocate(typeof(IRequestHandler<RegisterUserRequest, RegisterUserResponse>))]
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IUserPersistor _userPersistor;
        private readonly IBusService _busService;
        private readonly IServiceProvider _serviceProvider;

        public RegisterUserCommandHandler(IUserPersistor userPersistor, IBusService busService, IServiceProvider serviceProvider)
        {
            _userPersistor = userPersistor;
            _busService = busService;
            _serviceProvider = serviceProvider;
        }

        [LogTrace(returnType: typeof(RegisterUserResponse))]
        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
        {
            var domainResult = await _userPersistor.Add(new Domain.User.Entities.User(request.UserName)
            {
                DisplayName = request.DisplayName,
                PasswordHash = PasswordHelper.EncryptoPassword(request.Password)
            });

            if (domainResult.Success)
            {
                await _busService.Publish(
                    new UserRegisterdEvent
                    {
                        UserId = Guid.Parse(domainResult.Id.Code),
                        DisplayName = request.DisplayName
                    },
                    UserRegisterdEvent.RoutingKeyRegister);
            }
            
            return new RegisterUserResponse(domainResult.Message, domainResult.Success);
        }
    }
}
