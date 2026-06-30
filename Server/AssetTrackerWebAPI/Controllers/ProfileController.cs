using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController: ControllerBase
    {
       private readonly ProfileService _profileService;

            public ProfileController(ProfileService profileService)
            {
                _profileService = profileService;
                
            } 
            [HttpGet("{userId}")]
            public async Task<ActionResult<Profile>> GetProfile(string userId)
            {
                var profile = await _profileService.GetProfileByUserId(userId);
                if (profile == null)
                {
                    return NotFound();
                }
                return Ok(profile);
            }
    }
    
}
