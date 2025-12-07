using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public class GetUserByIdRequest: ICacheRequest
    {
        public GetUserByIdRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }

        public string Id { get => UserId;}
    }

    public record GetUserByIdResponse(string Message, bool Success, Domain.User.Entities.User? User): IResponse;
}
