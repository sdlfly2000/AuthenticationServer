using Common.Core.CQRS.Request;

namespace Application.Services.User.Requests
{
    public class GetUserByIdRequest : IRequest
    {
        public string UserId { get; set; }
    }
}
