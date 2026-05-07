using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlaidItemController: ControllerBase
    {
        private readonly plaidItemService _plaidItemService;
        private readonly plaidService _plaidService;
        private readonly ProfileService _profileService;
        public PlaidItemController(plaidItemService plaidItemService, plaidService plaidService, ProfileService profileService)
        {
            _plaidItemService = plaidItemService;
            _profileService = profileService;
            _plaidService = plaidService;
        }
        [HttpGet("{profileId}")]
        public async Task<IActionResult> GetPlaidItems(string profileId)
        {
            var plaidItems = await _plaidItemService.GetPlaidItems(profileId);
            return Ok(plaidItems);  
        }
        [HttpGet("{profileId}/{itemId}")]
        public async Task<IActionResult> GetPlaidItem(string profileId, string itemId)
        {
            var plaidItem = await _plaidItemService.GetPlaidItem(profileId, itemId);
            if (plaidItem == null)
            {
                return NotFound();      
            }
             return Ok(plaidItem);
        }    
        [HttpPost("AddPlaidItem")]
        public async Task<IActionResult> AddPlaidItem([FromBody] PlaidItemRequest plaidItemRequest)
        {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var profile = await _profileService.GetProfileByUserId(userId);
        if (profile == null) return NotFound("Profile not found");
        var accessToken = await _plaidService.ExchangeToken(plaidItemRequest.publicToken);
        await _plaidItemService.AddPlaidItem(accessToken, plaidItemRequest.institutionId, plaidItemRequest.institution, plaidItemRequest.itemId, profile.profileId);
        return Ok();

        }

    }
}