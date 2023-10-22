using AuthService.Actions;
using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateJWTAction _generateJWTAction;

        public AuthenticationController(
            UserManager<UserEntity> userManager,
            IGenerateJWTAction generateJWTAction)
        {
            _userManager = userManager;
            _generateJWTAction = generateJWTAction;
        }

        [HttpPost]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return NotFound("User does not Exist.");
            }

            if(!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Problem("Authentication Failed");
            }

            var jwt = _generateJWTAction.Generate(
                await _userManager.GetClaimsAsync(user),
                await _userManager.GetRolesAsync(user));

            return Ok(new AuthenticateResponse
            {
                ReturnUrl = request.ReturnUrl,
                JwtToken = jwt
            });
        }
    }
}
