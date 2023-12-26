using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowPolicy")]
    public class UserManagerController : ControllerBase
    {
        private readonly ILogger<UserManagerController> _logger;
        private readonly UserManager<UserEntity> _userManager;

        public UserManagerController(
            ILogger<UserManagerController> logger,
            UserManager<UserEntity> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var user = new UserEntity
            {
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(String.Join(',', result.Errors.Select(e => e.Code + ": " + e.Description)));
            }

            var userAddDisplayName = await _userManager.FindByIdAsync(user.Id.ToString());
            result = await _userManager.AddClaimAsync(
                userAddDisplayName!,
                new Claim(ClaimTypes.Name, request.DisplayName));

            if (!result.Succeeded)
            {
                return BadRequest(String.Join(',', result.Errors.Select(e => e.Code + ": " + e.Description)));
            }

            return Ok();
        }

        [HttpGet("Users")]
        [Authorize]
        public IEnumerable<UserEntity> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<UserModel?> GetUserByUserId([FromQuery] string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return default;
            }

            var claims = await _userManager.GetClaimsAsync(user);

            return new UserModel
            {
                Id = claims.Where(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Single().Value,
                Name = claims.Where(claim => claim.Type.Equals(ClaimTypes.Name)).Single().Value,
            };
        }
    }
}