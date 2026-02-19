using Application.Gateway.User.Models;
using Application.Services.ReqRes;

namespace Application.Gateway.User
{
    public interface IUserGateway
    {
        Task<RegisterUserResponse> Register(RegisterUserRawRequest request, CancellationToken token);

        Task<AssignAppResponse> AssignApp(AssignAppRequest request, CancellationToken token);
    }
}
