using AuthService.Actions;
using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IGenerateJWTAction _generateJWTAction;

        public AuthenticationController(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IGenerateJWTAction generateJWTAction)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                return Forbid();
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!signInResult.Succeeded)
            {
                return Forbid();
            }

            var jwt = _generateJWTAction.Generate(
                await _userManager.GetClaimsAsync(user),
                await _userManager.GetRolesAsync(user));

            return Ok(new AuthenticateResponse
            {
                ReturnUrl = request.ReturnUrl,
                UserId = user.Id.ToString(),
                JwtToken = jwt
            });
        }
    }
}
