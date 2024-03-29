using Application.Services.User.Requests;
using Application.Services.User.Responses;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>))]
    public class GetAllUsersQueryRequestHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request)
        {
            var users = await _userRepository.GetAllUsers();

            return new GetAllUsersQueryResponse
            {
                Users = users,
                Message = string.Empty,
                Success = true,
            };
        }
    }
}
