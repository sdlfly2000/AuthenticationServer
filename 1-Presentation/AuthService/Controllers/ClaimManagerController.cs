using Application.Services.ReqRes;
using AuthService.Models;
using Common.Core.CQRS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowPolicy")]
    public class ClaimManagerController(IEventBus eventBus) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddUserClaim([FromQuery] string id, [FromBody] AddUserClaimRequestModel request, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await eventBus.Send<AddUserClaimRequest, AddUserClaimResponse>(
                new AddUserClaimRequest(id, request.ClaimType.TypeName, request.Value), token);

            if (!response.Success) 
            { 
                return Problem(response.ErrorMessage); 
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserClaim([FromQuery] string id, [FromBody] UpdateUserClaimRequestModel request, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await eventBus.Send<UpdateUserClaimRequest, UpdateUserClaimResponse>(
                new UpdateUserClaimRequest(id, request.ClaimId, request.ClaimType.TypeName, request.Value), token);

            return response.Success
                ? Ok()
                : Problem(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserClaim([FromQuery] string id, [FromBody] DeleteUserClaimRequestModel request, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await eventBus.Send<DeleteUserClaimRequest, DeleteUserClaimResponse>(
                new DeleteUserClaimRequest(id, request.ClaimType.TypeName, request.Value), token);

            return response.Success
                ? Ok()
                : Problem(response.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetClaimByUserId([FromQuery] string id, CancellationToken token)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
            {
                return BadRequest(ModelState);
            }

            var userResponse = await eventBus.Send<GetUserByIdRequest, GetUserByIdResponse>(new GetUserByIdRequest(id), token);

            if (!userResponse.Success) 
            { 
                return Problem(userResponse.ErrorMessage); 
            }

            var userClaims = userResponse.User!.Claims.Select(claim => 
                new UserClaimModel { 
                    ClaimType = new ClaimTypeValues(TypeShortName: claim.Name.Split('/').Last(), TypeName: claim.Name),
                    ClaimId = claim.Id.Code,
                    Value = claim.Value,
                    IsFixed = claim.IsFixed
                });

            return Ok(userClaims);
        }

        [HttpGet]
        public async Task<IList<ClaimTypeValues>> GetClaimTypes(CancellationToken token)
        {
            var response = await eventBus.Send<GetClaimTypesRequest, GetClaimTypesResponse>(new GetClaimTypesRequest(), token);
            
            return response.Success 
                ? response.ClaimTypes
                : new List<ClaimTypeValues>();
        }
    }
}
