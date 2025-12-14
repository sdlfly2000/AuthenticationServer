using Application.Services.User.ReqRes;
using Common.Core.Authentication;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Infra.Core;
using Infra.Core.LogTrace;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.User.CommandHandlers
{
    [ServiceLocate(typeof(IRequestHandler<AuthenticateRequest, AuthenticateResponse>))]
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public AuthenticateCommandHandler(
            IUserRepository userRepository, 
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        public async Task<AuthenticateResponse> Handle(AuthenticateRequest request)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return new AuthenticateResponse("Password is empty", false);
            }

            var passwordEncry = PasswordHelper.EncryptoPassword(request.Password);

            var user = await _userRepository.FindUserByUserNamePwd(request.UserName, passwordEncry);

            if(user == null)
            {
                return new AuthenticateResponse("User Name or Password does not match.", false);
            }

            var claims = user.Claims.Select(claim => new Claim(claim.Name, claim.Value, claim.ValueType)).ToList();

            claims.Add(new Claim(ClaimTypes.UserData, request.UserAgent));

            var jwt = GenerateJwt(claims);

            return new AuthenticateResponse(string.Empty, true, jwt, user.Id.Code, user.DisplayName);
        }

        private string GenerateJwt(IList<Claim> claims)
        {
            var jwtOptions = _configuration.GetSection("JWT").Get<JWTOptions>();

            if (jwtOptions == null)
            {
                jwtOptions = new JWTOptions
                {
                    ExpireSeconds = 86400, // 24 hr
                    Issuer = "AuthenticationService",
                    SigningKey = "fasdfad&9045dafz222#fadpio@0232121582"
                };
            }

            DateTime expires = DateTime.Now.AddSeconds(jwtOptions.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                expires: expires,
                signingCredentials: credentials,
                claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
