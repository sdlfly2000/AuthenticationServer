namespace AuthService.Models
{
    public class UpdateUserClaimRequest
    {
        public UserClaim oldClaim { get; set; }
        public UserClaim newClaim { get; set;}
        public string userId {  get; set; }
    }
}
