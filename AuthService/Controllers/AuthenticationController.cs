using AuthService.Actions;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateAction _authenticateAction;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationController(
            IAuthenticateAction authenticateAction,
            IMemoryCache memoryCache)
        {
            _authenticateAction = authenticateAction;
            _memoryCache = memoryCache;
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

        [HttpGet]
        [EnableCors("AllowPolicy")]
        [Authorize]
        public IActionResult Logout()
        {
            if(!HttpContext.Items.TryGetValue("CacheJwtKey", out var key))
            {
                return BadRequest("Failed to deactivate login");
            }

            if (null == key)
            {
                return BadRequest("Failed to deactivate login");
            };

            _memoryCache.Set(key, false);

            return Ok();
        }
    }
}
