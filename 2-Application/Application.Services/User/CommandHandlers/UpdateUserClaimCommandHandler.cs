using Application.Services.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.LogTrace;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>))]
    public class UpdateUserClaimCommandHandler(
        IUserRepository userRepository,
        IUserPersistor userPersistor,
        IServiceProvider serviceProvider)
        : IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [LogTrace(returnType: typeof(UpdateUserClaimResponse))]
        public async Task<UpdateUserClaimResponse> Handle(UpdateUserClaimRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.Find((UserReference)request.UserId, cancellationToken);

            user!.UpdateClaim(request.ClaimId, request.ClaimValue);

            var result = await userPersistor.Update(user);

            return new UpdateUserClaimResponse(result.Message, result.Success);
        }
    }
}
