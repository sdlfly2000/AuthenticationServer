using Application.Services.ReqRes;

namespace AuthService.Models
{
    public class DeleteUserClaimRequestModel
    {
        public ClaimTypeValues ClaimType { get; set; }
        public string Value { get; set; }
    }
}
