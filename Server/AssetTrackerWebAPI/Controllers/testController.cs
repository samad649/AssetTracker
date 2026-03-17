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
        [HttpPost("CreateProfile")]
        public async Task<ActionResult<Profile>> CreateProfile([FromBody] Profile profile)
        {
            var createdProfile = await _profileService.CreateProfileAsync(profile);
            return Ok(createdProfile);
        }
    }
}
