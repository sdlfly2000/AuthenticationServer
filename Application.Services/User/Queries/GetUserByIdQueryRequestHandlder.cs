using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.Cache;
using Infra.Core.CacheFieldNames;
using Infra.Core.LogTrace;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>))]
    public class GetUserByIdQueryRequestHandlder : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider; // keep it for LogTrace

        public GetUserByIdQueryRequestHandlder(IUserRepository userRepository, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        //[Cache(key: CacheFieldNames.User, subKeyType: typeof(GetUserByIdRequest))]
        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request)
        {
            var user = await _userRepository.Find(UserReference.Create(request.UserId));

            if (user == null) 
            {
                return new GetUserByIdResponse($"User {request.UserId} is Not Found", false, default);
            }

            return new GetUserByIdResponse(string.Empty, true, user);
        }
    }
}
