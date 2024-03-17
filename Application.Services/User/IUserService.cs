using Application.Services.User.Requests;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User
{
    public interface IUserService
    {
        Task<ApplicationResult> Register(RegisterUserRequest request);
    }
}
