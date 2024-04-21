using Common.Core.CQRS.Request;
using Domain.User.Repositories;
using Application.Services.User.ReqRes;

namespace Application.Services.User.Queries
{
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

            return new GetAllUsersQueryResponse(string.Empty, true, users);
        }
    }
}
