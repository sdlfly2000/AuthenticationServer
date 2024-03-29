using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.Responses
{
    public class GetAllUsersQueryResponse : ApplicationResponse, IResponse
    {
        public List<Domain.User.Entities.User> Users { get; set; } = new List<Domain.User.Entities.User>();
    }
}
