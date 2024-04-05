using Application.Services.User.ReqRes;
using AuthService.Models;
using Common.Core.CQRS;
using Infra.Core;
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
        private readonly IEventBus _eventBus;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationController(
                    IEventBus eventBus,
                    IMemoryCache memoryCache)
        {
            _eventBus = eventBus;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        [EnableCors("AllowPolicy")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var password = PasswordHelper.ExtractPwdWithTimeVerification(request.Password);

            if (password == null)
            {
                return BadRequest();
            }

            var authenticateResponse = await _eventBus.Send<AuthenticateRequest, AuthenticateResponse>(
                new AuthenticateRequest(
                    request.UserName,
                    password, 
                    HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString(), 
                    HttpContext.Request.Headers.UserAgent.ToString()));

            if(authenticateResponse != null && authenticateResponse.Success)
            {
                return Ok(new AuthenticateResponseModel
                {
                    JwtToken = authenticateResponse.JwtToken,
                    UserId = authenticateResponse.UserId,
                    UserDisplayName = authenticateResponse.UserDisplayName,
                    ReturnUrl = request.ReturnUrl
                });
            }

            return Forbid();
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
