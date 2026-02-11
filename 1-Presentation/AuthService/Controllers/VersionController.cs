using Infra.Core.Version;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowPolicy")]
public class VersionController : ControllerBase
{
    public IActionResult Index()
    {                       
        return Ok(VersionManger.GetVersion());
    }
}