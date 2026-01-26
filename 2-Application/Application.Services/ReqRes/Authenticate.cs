using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record AuthenticateRequest(string UserName, string RawPassword, string UserAgent) : AppRequest, IRequest;

    public record AuthenticateResponse: AppResponse, IResponse
    {
        public AuthenticateResponse(string errorMessage, bool success, string jwtToken, string userId, string userDisplayName) : base(errorMessage, success) 
        {
            JwtToken = jwtToken;
            UserId = userId;
            UserDisplayName = userDisplayName;
        }

        public AuthenticateResponse(string errorMessage, bool success) : this(errorMessage, success, string.Empty, string.Empty, string.Empty)
        {
        }

        public string JwtToken { get; init; } = string.Empty;

        public string UserId { get; init; } = string.Empty;

        public string UserDisplayName { get; init; } = string.Empty;
    }
}
