namespace AuthService.Models
{
    public class RegisterUserModelRequest
    {
        public string UserName { get; set; }
        public string PasswordEncrypto { get; set; }
        public string DisplayName {  get; set; }
    }
}
