using AuthService.Models;

namespace AuthService.Actions
{
    public interface IAuthenticateAction
    {
        Task<AuthenticateResponse?> AuthenticateAndGenerateJwt(AuthenticateRequest request, HttpContext context);
    }
}
