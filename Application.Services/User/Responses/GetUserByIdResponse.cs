using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.Responses
{
    public class GetUserByIdResponse : ApplicationResponse, IResponse
    {
        public required Domain.User.Entities.User User { get; set; }
    }
}
