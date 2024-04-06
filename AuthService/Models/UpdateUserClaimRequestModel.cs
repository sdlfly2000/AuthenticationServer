namespace AuthService.Models
{
    public class UpdateUserClaimRequestModel
    {
        public UserClaim oldClaim { get; set; }
        public UserClaim newClaim { get; set;}
        public string userId {  get; set; }
    }
}
