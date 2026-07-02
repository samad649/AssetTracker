using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlaidController: ControllerBase
    {
        private readonly plaidService _plaidService;
        private readonly ProfileService _profileService;

        public PlaidController(plaidService plaidService, ProfileService profileService)
        {
            _plaidService = plaidService;
            _profileService = profileService;
        }
        [HttpPost("createLinkToken")]
        public async Task<IActionResult> CreateLinkToken([FromBody] CreateLinkTokenRequest request)
        {
            try
            {
                var token = await _plaidService.CreateLinkToken(request.userId);
                return Ok(new { link_token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpDelete("deleteItem/{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string itemId)
        {
             var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();
            var profile = await _profileService.GetProfileByUserId(userId);
            if(profile == null) return NotFound("Profile not found");

            var profileId = profile.profileId;

            try
            {
                await _plaidService.RemoveItem(profileId, itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, detail = ex.ToString() });
            }
        }
    }
}