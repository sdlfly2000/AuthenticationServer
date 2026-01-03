using Application.Services.User.ReqRes;

namespace AuthService.Models
{
    public class AddUserClaimRequestModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }
    }
}
