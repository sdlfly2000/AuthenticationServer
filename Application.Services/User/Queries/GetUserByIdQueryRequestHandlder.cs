using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Domain.User.ValueObjects;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>))]
    public class GetUserByIdQueryRequestHandlder : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryRequestHandlder(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
