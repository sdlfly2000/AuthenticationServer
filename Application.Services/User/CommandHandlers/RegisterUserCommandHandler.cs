using Application.Services.User.Requests;
using Application.Services.User.Responses;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;

namespace Application.Services.User.Commands
{
    [ServiceLocate(typeof(IRequestHandler<RegisterUserRequest, RegisterUserResponse>))]
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IUserPersistor _userPersistor;

        public RegisterUserCommandHandler(IUserPersistor userPersistor)
        {
            _userPersistor = userPersistor;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
        {
            var domainResult = await _userPersistor.Add(new Domain.User.Entities.User(request.UserName)
            {
                DisplayName = request.DisplayName,
                PasswordHash = request.PasswordHash
            });

            return new RegisterUserResponse
            {
                Message = domainResult.Message,
                Success = domainResult.Success,
            };
        }
    }
}
