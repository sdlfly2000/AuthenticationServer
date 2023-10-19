using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Actions
{
    public class GenerateJWTAction : IGenerateJWTAction
    {
        private readonly JWTOptions _options;

        public GenerateJWTAction(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(IList<Claim> claims, IList<string> roles)
        {
            DateTime expires = DateTime.Now.AddSeconds(_options.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(_options.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                expires: expires,
                signingCredentials: credentials,
                claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
