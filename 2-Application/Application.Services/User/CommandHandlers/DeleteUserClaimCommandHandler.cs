using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.CatchException;
using Infra.Core.LogTrace;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse>))]
    public class DeleteUserClaimCommandHandler : IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPersistor _userPersistor;
        private readonly IServiceProvider _serviceProvider;

        public DeleteUserClaimCommandHandler(
            IUserRepository userRepository,
            IUserPersistor userPersistor,
            IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _userPersistor = userPersistor;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        [CatchAppException(returnType: typeof(DeleteUserClaimResponse))]
        public async Task<DeleteUserClaimResponse> Handle(DeleteUserClaimRequest request)
        {
            var user = await _userRepository.Find((UserReference)request.UserId);

            ArgumentNullException.ThrowIfNull(user, $"Cannot find User by {request.UserId}.");
             
            user.DeleteClaim(request.ClaimType, request.ClaimValue);

            var result = await _userPersistor.Update(user);

            return new DeleteUserClaimResponse(result.Message, result.Success);
        }
    }
}
