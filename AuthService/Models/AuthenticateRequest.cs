namespace AuthService.Models
{
    public class AuthenticateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set;}
    }
}
