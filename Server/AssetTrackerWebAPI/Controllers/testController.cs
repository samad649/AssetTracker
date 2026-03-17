using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("GetMockProfile")]
        public ActionResult<Profile> CreateProfile()
        {
            var createdProfile = _profileService.GetMockProfile();
            return Ok(createdProfile);
        }
    }
}
