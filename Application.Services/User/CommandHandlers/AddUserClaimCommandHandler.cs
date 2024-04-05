using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>))]
    public class AddUserClaimCommandHandler : IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>
    {
        public AddUserClaimCommandHandler()
        {
            
        }

        public Task<AddUserClaimResponse> Handle(AddUserClaimRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
