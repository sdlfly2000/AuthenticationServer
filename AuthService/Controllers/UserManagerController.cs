using Application.Services.User;
using Application.Services.User.Requests;
using AuthService.Models;
using Infra.Core;
using Infra.Database.Entities;
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
        private readonly IUserService _userService;

        public UserManagerController(
            ILogger<UserManagerController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
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

            var result = await _userService.Register(registerUserRequest);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
                        
            //var userAddDisplayName = await _userManager.FindByIdAsync(user.Id.ToString());
            //result = await _userManager.AddClaimAsync(
            //    userAddDisplayName!,
            //    new Claim(ClaimTypes.Name, request.DisplayName));

            //if (!result.Succeeded)
            //{
            //    return BadRequest(String.Join(',', result.Errors.Select(e => e.Code + ": " + e.Description)));
            //}

            return Ok();
        }

        [HttpGet("Users")]
        [Authorize]
        public IEnumerable<UserEntity> GetUsers()
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