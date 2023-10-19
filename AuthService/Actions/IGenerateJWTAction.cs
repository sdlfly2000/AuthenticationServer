using System.Security.Claims;

namespace AuthService.Actions
{
    public interface IGenerateJWTAction
    {
        string Generate(IList<Claim> claims, IList<string> roles);
    }
}
