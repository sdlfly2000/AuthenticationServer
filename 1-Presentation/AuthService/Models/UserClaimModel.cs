using Application.Services.User.ReqRes;

namespace AuthService.Models
{
    public class UserClaimModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }
    }
}
