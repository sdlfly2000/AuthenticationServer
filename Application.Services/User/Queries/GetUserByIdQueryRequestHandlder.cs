using Application.Services.User.Requests;
using Application.Services.User.Responses;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;

namespace Application.Services.User.Queries
{
    [ServiceLocate(typeof(IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>))]
    public class GetUserByIdQueryRequestHandlder : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        public Task<GetUserByIdResponse> Handle(GetUserByIdRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
