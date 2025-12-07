namespace Application.Gateway.User.Models
{
    public record RegisterUserRawRequest(string UserName, string rawPassword, string DisplayName);
}
