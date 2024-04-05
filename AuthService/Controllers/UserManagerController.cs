using Application.Services.User.ReqRes;
using AuthService.Models;
using Common.Core.CQRS;
using Domain.User.Entities;
using Infra.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowPolicy")]
    public class UserManagerController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public UserManagerController(
            IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel request)
        {
            var registerUserRequest = new RegisterUserRequest(
                request.UserName, 
                PasswordHelper.ExtractPwdWithTimeVerification(request.PasswordEncrypto) ?? string.Empty, 
                request.DisplayName) ;

            var result = await _eventBus.Send<RegisterUserRequest, RegisterUserResponse>(registerUserRequest);

            if (result != null && !result.Success)
            {
                return BadRequest(result.Message);
            }
                        
            return Ok();
        }

        [HttpGet("Users")]
        [Authorize]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var getAllUsersRequest = new GetAllUsersQueryRequest();
            var response = await _eventBus.Send<GetAllUsersQueryRequest, GetAllUsersQueryResponse>(getAllUsersRequest);
            
            return response.Users;
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<UserModel?> GetUserByUserId([FromQuery] string id)
        {
            var request = new GetUserByIdRequest(id);
            var response = await _eventBus.Send<GetUserByIdRequest, GetUserByIdResponse>(request);

            return response.Success 
                ? new UserModel(response.User!.Id.Code, response.User.DisplayName)
                : default;
        }
    }
}