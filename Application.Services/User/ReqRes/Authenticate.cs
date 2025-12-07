using Infra.Core.ApplicationBasics;

namespace Application.Services.User.ReqRes
{
    public record AuthenticateRequest(string UserName, string Password, string UserAgent) : AppRequest;

    public record AuthenticateResponse: AppResponse
    {
        public AuthenticateResponse(string errorMessage, bool Success, string JwtToken, string UserId, string UserDisplayName) : base(errorMessage, Success) 
        { 
        }

        public AuthenticateResponse(string errorMessage, bool Success) : this(errorMessage, Success, string.Empty, string.Empty, string.Empty)
        {
        }
    }
}
