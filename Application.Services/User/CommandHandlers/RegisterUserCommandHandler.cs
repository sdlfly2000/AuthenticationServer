using Application.Services.User.Requests;
using Application.Services.User.Responses;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;

namespace Application.Services.User.Commands
{
    [ServiceLocate(typeof(IRequestHandler<RegisterUserRequest, RegisterUserResponse>))]
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
        {
            var domainResult = await _userRepository.Add(new Domain.User.Entities.User(request.UserName)
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
