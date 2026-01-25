using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.Authorizations.Attributes;
using Domain.User.Repositories;
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

        [LogTrace(returnType: typeof(GetAllUsersQueryResponse))]
        [ActiAuthorize(Right: "ListAllUser", Role: null)]
        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request)
        {
            var users = await _userRepository.GetAllUsers();

            return new GetAllUsersQueryResponse(string.Empty, true, users);
        }
    }
}
