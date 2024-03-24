using Application.Services.User;
using Application.Services.User.Requests;
using Application.Services.User.Responses;
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
        private readonly ILogger<UserManagerController> _logger;
        private readonly IEventBus _eventBus;

        public UserManagerController(
            ILogger<UserManagerController> logger,
            IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModelRequest request)
        {
            var registerUserRequest = new RegisterUserRequest 
            { 
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                PasswordHash = PasswordHelper.ExtractPwdWithTimeVerification(request.PasswordEncrypto) ?? string.Empty
            };

            var result = (await _eventBus.Send<RegisterUserRequest, RegisterUserResponse>(registerUserRequest)) as RegisterUserResponse;

            if (result != null && !result.Success)
            {
                return BadRequest(result.Message);
            }
                        
            return Ok();
        }

        [HttpGet("Users")]
        [Authorize]
        public IEnumerable<User> GetUsers()
        {
            //return _userManager.Users.ToList();
            return null;
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<UserModel?> GetUserByUserId([FromQuery] string id)
        {
            //var user = await _userManager.FindByIdAsync(id);

            //if(user == null)
            //{
            //    return default;
            //}

            //var claims = await _userManager.GetClaimsAsync(user);

            //return new UserModel
            //{
            //    Id = claims.Where(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Single().Value,
            //    Name = claims.Where(claim => claim.Type.Equals(ClaimTypes.Name)).Single().Value,
            //};

            return null;
        }
    }
}