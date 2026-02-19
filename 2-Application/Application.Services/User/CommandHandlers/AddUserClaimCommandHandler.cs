using Application.Services.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.LogTrace;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>))]
    public class AddUserClaimCommandHandler(
        IUserRepository userRepository,
        IUserPersistor userPersistor,
        IServiceProvider serviceProvider)
        : IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [LogTrace(returnType: typeof(AddUserClaimResponse))]
        public async Task<AddUserClaimResponse> Handle(AddUserClaimRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.Find((UserReference)request.UserId);

            user.AddClaim(request.ClaimType, request.ClaimValue);

            var updateResult = await userPersistor.Update(user);

            return new AddUserClaimResponse(updateResult.Message, updateResult.Success);
        }
    }
}
