using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = new UserEntity
            {
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded
                ? Ok()
                : BadRequest(String.Join(',', result.Errors.Select(e => e.Code + ": " + e.Description)));
        }

        [HttpGet("Users")]
        [Authorize]
        public IEnumerable<UserEntity> GetUsers()
        {
            return _userManager.Users.ToList();
        }
    }
}