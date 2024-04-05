using Application.Services.User.ReqRes;
using AuthService.Models;
using Common.Core.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowPolicy")]
    [Authorize]
    public class ClaimManagerController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public ClaimManagerController(
            IEventBus eventBus )
        {
            _eventBus = eventBus;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddUserClaim([FromBody] AddUserClaimRequest request)
        //{
        //    var user = await _userManager.FindByNameAsync(request.UserName);
        //    if (user == null) { return Problem("User does not exist."); }

        //    var claimName = new Claim(ClaimTypes.Name, request.Claim, ClaimValueTypes.String);
        //    var claimNameIdentifiers = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String);

        //    var result = await _userManager.AddClaimsAsync(user, new[] { claimName, claimNameIdentifiers});
        //    if (!result.Succeeded) { return Problem("Failed to Add Claim"); }

        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateUserClaim([FromBody] UpdateUserClaimRequest request)
        //{
        //    var user = await _userManager.FindByIdAsync(request.userId);
        //    if (user == null) { return NotFound("User does not exist."); }

        //    var allClaims = await _userManager.GetClaimsAsync(user);
        //    var preClaim = allClaims.Single(
        //        claim => 
        //            (claim.Value == request.oldClaim.value) && 
        //            (claim.Type.Split('/').Last() == request.oldClaim.type));

        //    var newClaim = new Claim(preClaim.Type, request.newClaim.value, preClaim.ValueType);

        //    var result = await _userManager.ReplaceClaimAsync(user, preClaim, newClaim);     

        //    return result.Succeeded
        //        ? Ok()
        //        : Problem(title: string.Join(";", result.Errors), statusCode:500);
        //}

        [HttpGet]
        public async Task<IActionResult> GetClaimByUserId([FromQuery] string id)
        {
            var userReponse = await _eventBus.Send<GetUserByIdRequest, GetUserByIdResponse>(new GetUserByIdRequest(id));

            if (!userReponse.Success) 
            { 
                return Problem(userReponse.Message); 
            }

            var userClaims = userReponse.User!.Claims.Select(claim => new UserClaimModel { Type = claim.Name.Split('/').Last(), Value = claim.Value });

            return Ok(userClaims);
        }
    }
}
