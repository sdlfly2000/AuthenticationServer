using AuthService.Models;
using Azure.Core;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace AuthService.Actions
{
    public class AuthenticateAction : IAuthenticateAction
    {
        private readonly int ALLOW_DELAY_IN_SEC = 2;

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IGenerateJWTAction _generateJWTAction;
        private readonly IConfiguration _configuration;

        public AuthenticateAction(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IGenerateJWTAction generateJWTAction,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateJWTAction = generateJWTAction;

            _configuration = configuration;
            ALLOW_DELAY_IN_SEC = int.Parse(_configuration.GetSection("AuthServiceConfigure:AllowMaxDelayInSec").Value!);
        }

        public async Task<AuthenticateResponse?> AuthenticateAndGenerateJwt(AuthenticateRequest request, HttpContext context)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return null;
            }

            var password = ExtractPwdWithTimeVerification(request.Password);

            if (password == null) return null;

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
            {
                return null;
            }

            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(ClaimTypes.Uri, context.Connection.RemoteIpAddress!.MapToIPv4().ToString()));
            claims.Add(new Claim(ClaimTypes.UserData, context.Request.Headers.UserAgent.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, request.UserName));
            var jwt = _generateJWTAction.Generate(
                claims,
                await _userManager.GetRolesAsync(user));

            return new AuthenticateResponse
            {
                ReturnUrl = request.ReturnUrl,
                UserId = user.Id.ToString(),
                UserDisplayName = claims.Single(claim => claim.Type.Equals(ClaimTypes.Name)).Value,
                JwtToken = jwt
            };
        }

        #region Private Methods

        private string ConvertBase64ToString(string base64)
        {
            if (base64.Length % 4 != 0)
                base64 += new String('=', 4 - base64.Length % 4);

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(base64));
        }

        private string? ExtractPwdWithTimeVerification(string orginalPwd)
        {
            var pwdCompounds = ConvertBase64ToString(orginalPwd).Split('|');
            var password = pwdCompounds[0];
            var datetimeSent = long.Parse(pwdCompounds[1])/1000;

            if (((DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds - datetimeSent) > ALLOW_DELAY_IN_SEC)
            {
                return null;
            }

            return password;
        }

        #endregion
    }
}
