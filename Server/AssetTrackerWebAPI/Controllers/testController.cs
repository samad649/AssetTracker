using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public TestController(ProfileService profileService)
        {
            _profileService = profileService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var profile = _profileService.CreateProfile();
            return Ok(profile);
        }
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<Account>> CreateAccount([FromBody] Account account)
        {
            var createdAccount = await _profileService.CreateAccountAsync(account);
            return Ok(createdAccount);
        }
    }
}
