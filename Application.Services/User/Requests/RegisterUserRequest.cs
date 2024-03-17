namespace Application.Services.User.Requests
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
    }
}
