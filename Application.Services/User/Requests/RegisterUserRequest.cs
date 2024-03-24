using Common.Core.CQRS.Request;

namespace Application.Services.User.Requests
{
    public class RegisterUserRequest: IRequest
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
    }
}
