using AuthService.Actions;
using AuthService.Models;
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
        public IActionResult LogOut()
        {
            var ipAdress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            if (ipAdress == null) 
            {
                return BadRequest("Failed to have Ip Address");
            }

            if(!_memoryCache.Set(ipAdress, false))
            {
                return BadRequest("Failed to deactivate login");
            };               

            return Ok();
        }
    }
}
