namespace AuthService.Models
{
    public class AddUserClaimRequestModel
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
