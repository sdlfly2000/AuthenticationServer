﻿using Application.Services.User.ReqRes;
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
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
                    ILogger<AuthenticationController> logger,
                    IEventBus eventBus,
                    IMemoryCache memoryCache)
        {
            _eventBus = eventBus;
            _memoryCache = memoryCache;
            _logger = logger;
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
                _logger.LogWarning($"{nameof(AuthenticationController)}: password is null.");
                return BadRequest();
            }

            var authenticateResponse = await _eventBus.Send<AuthenticateRequest, AuthenticateResponse>(
                new AuthenticateRequest(
                    request.UserName,
                    password, 
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

            if (authenticateResponse != null && authenticateResponse.Success == false)
            {
                _logger.LogWarning($"{nameof(AuthenticationController)}: failed due to {authenticateResponse.Message}.");
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
