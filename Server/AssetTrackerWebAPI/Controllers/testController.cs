using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
namespace AssetTrackerWebAPI.Controllers
{
     [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
   
        [HttpGet("check-token")]
        public IActionResult CheckToken()
        {
            var header = Request.Headers["Authorization"].ToString();
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new { header, isAuthenticated, claims });
        }
    }
}
