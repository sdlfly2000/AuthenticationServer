using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>))]
    public class AddUserClaimCommandHandler : IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPersistor _userPersistor;

        public AddUserClaimCommandHandler(IUserRepository userRepository, IUserPersistor userPersistor)
        {
            _userRepository = userRepository;
            _userPersistor = userPersistor;
        }

        public async Task<AddUserClaimResponse> Handle(AddUserClaimRequest request)
        {
            var user = await _userRepository.Find((UserReference)request.UserId);
            user!.AddClaim(request.ClaimType, request.ClaimValue);

            var updateResult = await _userPersistor.Update(user);

            return new AddUserClaimResponse(updateResult.Message, updateResult.Success);
        }
    }
}
