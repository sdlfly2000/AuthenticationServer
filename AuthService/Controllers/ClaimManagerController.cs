using AuthService.Models;
using Infra.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClaimManagerController : ControllerBase
    {
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly UserManager<UserEntity> _userManager;

        public ClaimManagerController(
            RoleManager<RoleEntity> roleManager, 
            UserManager<UserEntity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserClaim([FromBody] AddUserClaimRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) { return Problem("User does not exist."); }

            var claimName = new Claim(ClaimTypes.Name, request.Claim, ClaimValueTypes.String);
            var claimNameIdentifiers = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String);

            var result = await _userManager.AddClaimsAsync(user, new[] { claimName, claimNameIdentifiers});
            if (!result.Succeeded) { return Problem("Failed to Add Claim"); }

            return Ok();
        }
    }
}
