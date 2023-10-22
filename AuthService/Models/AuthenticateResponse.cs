namespace AuthService.Models
{
    public class AuthenticateResponse
    {
        public string JwtToken { get; set; }
        public string ReturnUrl { get; set; }
    }
}
