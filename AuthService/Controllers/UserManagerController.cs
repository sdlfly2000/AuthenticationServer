using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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

            var createResult = await _userManager.CreateAsync(user);       

            var addPwdResult = await _userManager.AddPasswordAsync(user, request.Password);

            return createResult.Succeeded && addPwdResult.Succeeded
                ? Ok()
                : BadRequest();
        }

        [HttpGet("Users")]
        public IEnumerable<UserEntity> GetUsers()
        {
            return _userManager.Users.ToList();
        }

    }
}