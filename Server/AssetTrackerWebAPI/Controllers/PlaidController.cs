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

        public PlaidController(plaidService plaidService)
        {
            _plaidService = plaidService;
        }
        [AllowAnonymous]
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
    }
}