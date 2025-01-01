using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public record AuthenticateRequest(string UserName, string Password, string UserAgent) : IRequest;

    public class AuthenticateResponse: IResponse
    {
        public AuthenticateResponse(string Message, bool Success, string JwtToken, string UserId, string UserDisplayName)
        {
            this.Message = Message;
            this.Success = Success;
            this.JwtToken = JwtToken;
            this.UserId = UserId;
            this.UserDisplayName = UserDisplayName;
        }

        public AuthenticateResponse(string Message, bool Success) : this(Message, Success, string.Empty, string.Empty, string.Empty)
        {
        }

        public string Message { get; private set; }
        public bool Success { get; private set; }
        public string JwtToken { get; private set; }
        public string UserId { get; private set; }
        public string UserDisplayName { get; private set; }
    }
}
