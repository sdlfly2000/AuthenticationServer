using Application.Services.ReqRes;

namespace AuthService.Models
{
    public class UpdateUserClaimRequestModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }
        public string ClaimId { get; set; }
    }
}
