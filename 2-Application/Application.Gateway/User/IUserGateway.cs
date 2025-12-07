using Application.Gateway.User.Models;
using Application.Services.User.ReqRes;

namespace Application.Gateway.User
{
    public interface IUserGateway
    {
        Task<RegisterUserResponse> Register(RegisterUserRawRequest request, CancellationToken token);
    }
}
