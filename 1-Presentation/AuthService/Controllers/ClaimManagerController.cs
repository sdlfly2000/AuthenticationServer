using Application.Services.User.ReqRes;
using AuthService.Models;
using Common.Core.CQRS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowPolicy")]
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _eventBus.Send<AddUserClaimRequest, AddUserClaimResponse>(
                new AddUserClaimRequest(id, request.ClaimType.TypeName, request.Value));

            if (!response.Success) 
            { 
                return Problem(response.ErrorMessage); 
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserClaim([FromQuery] string id, [FromBody] UpdateUserClaimRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _eventBus.Send<UpdateUserClaimRequest, UpdateUserClaimResponse>(
                new UpdateUserClaimRequest(id, request.ClaimType.TypeName, request.Value));

            return response.Success
                ? Ok()
                : Problem(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserClaim([FromQuery] string id, [FromBody] DeleteUserClaimRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _eventBus.Send<DeleteUserClaimRequest, DeleteUserClaimResponse>(
                new DeleteUserClaimRequest(id, request.ClaimType.TypeName, request.Value));

            return response.Success
                ? Ok()
                : Problem(response.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetClaimByUserId([FromQuery] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userResponse = await _eventBus.Send<GetUserByIdRequest, GetUserByIdResponse>(new GetUserByIdRequest(id));

            if (!userResponse.Success) 
            { 
                return Problem(userResponse.ErrorMessage); 
            }

            var userClaims = userResponse.User!.Claims.Select(claim => 
                new UserClaimModel { 
                    ClaimType = new ClaimTypeValues(TypeShortName: claim.Name.Split('/').Last(), TypeName: claim.Name), 
                    Value = claim.Value,
                    IsFixed = claim.IsFixed
                });

            return Ok(userClaims);
        }

        [HttpGet]
        public async Task<IList<ClaimTypeValues>> GetClaimTypes()
        {
            var response = await _eventBus.Send<GetClaimTypesRequest, GetClaimTypesResponse>(new GetClaimTypesRequest());
            
            return response.Success 
                ? response.ClaimTypes
                : new List<ClaimTypeValues>();
        }
    }
}
