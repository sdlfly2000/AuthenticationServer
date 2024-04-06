using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>))]
    public class UpdateUserClaimCommandHandler : IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPersistor _userPersistor;

        public UpdateUserClaimCommandHandler(
            IUserRepository userRepository,
            IUserPersistor userPersistor)
        {
            _userRepository = userRepository;
            _userPersistor = userPersistor;
        }

        public async Task<UpdateUserClaimResponse> Handle(UpdateUserClaimRequest request)
        {
            var user = await _userRepository.Find((UserReference)request.UserId);

            user!.UpdateClaim(request.ClaimType, request.ClaimValue);

            var result = await _userPersistor.Update(user);

            return new UpdateUserClaimResponse(result.Message, result.Success);
        }
    }
}
