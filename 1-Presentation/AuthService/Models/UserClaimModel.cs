using Application.Services.ReqRes;

namespace AuthService.Models
{
    public class UserClaimModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }

        public bool IsFixed { get; set; }
    }
}
