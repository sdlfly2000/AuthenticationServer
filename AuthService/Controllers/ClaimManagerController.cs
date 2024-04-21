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

        [HttpPost]
        public async Task<IActionResult> AddUserClaim([FromQuery] string id, [FromBody] AddUserClaimRequestModel request)
        {
            var response = await _eventBus.Send<AddUserClaimRequest, AddUserClaimResponse>(
                new AddUserClaimRequest(id, request.typeName, request.value));

            if (!response.Success) 
            { 
                return Problem("Failed to Add Claim"); 
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserClaim([FromQuery] string id, [FromBody] UpdateUserClaimRequestModel request)
        {
            var response = await _eventBus.Send<UpdateUserClaimRequest, UpdateUserClaimResponse>(
                new UpdateUserClaimRequest(id, request.typeName, request.value));

            return response.Success
                ? Ok()
                : Problem(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetClaimByUserId([FromQuery] string id)
        {
            var userReponse = await _eventBus.Send<GetUserByIdRequest, GetUserByIdResponse>(new GetUserByIdRequest(id));

            if (!userReponse.Success) 
            { 
                return Problem(userReponse.Message); 
            }

            var userClaims = userReponse.User!.Claims.Select(claim => new UserClaimModel { ShortTypeName = claim.Name.Split('/').Last(), Value = claim.Value, TypeName = claim.Name });

            return Ok(userClaims);
        }
    }
}
