namespace AuthService.Models
{
    public class RegisterUserRequestModel
    {
        public string UserName { get; set; }
        public string PasswordEncrypto { get; set; }
        public string DisplayName {  get; set; }
    }
}
