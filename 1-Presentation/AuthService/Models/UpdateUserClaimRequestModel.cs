using Application.Services.User.ReqRes;

namespace AuthService.Models
{
    public class UpdateUserClaimRequestModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }
    }
}
