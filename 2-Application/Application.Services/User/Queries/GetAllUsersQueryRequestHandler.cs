using Common.Core.CQRS.Request;
using Domain.User.Repositories;
using Application.Services.User.ReqRes;
using Common.Core.DependencyInjection;
using Infra.Core.LogTrace;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>))]
    public class GetAllUsersQueryRequestHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider; //Keep it for LogTrace

        public GetAllUsersQueryRequestHandler(IUserRepository userRepository, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request)
        {
            var users = await _userRepository.GetAllUsers();

            return new GetAllUsersQueryResponse(string.Empty, true, users);
        }
    }
}
