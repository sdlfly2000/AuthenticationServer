using Application.Services.User.ReqRes;

namespace AuthService.Models
{
    public class AddUserClaimRequestModel
    {
        public ClaimTypeValues claimType { get; set; }
        public string value { get; set; }
    }
}
