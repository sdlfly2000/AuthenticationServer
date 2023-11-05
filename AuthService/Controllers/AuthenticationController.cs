using AuthService.Actions;
using AuthService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateAction _authenticateAction;

        public AuthenticationController(IAuthenticateAction authenticateAction)
        {
            _authenticateAction = authenticateAction;
        }

        [HttpPost]
        [EnableCors("AllowPolicy")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResult = await _authenticateAction.AuthenticateAndGenerateJwt(request, HttpContext);

            return authResult != null
                ? Ok(authResult)
                : Forbid();
        }
    }
}
