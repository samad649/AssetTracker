using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
namespace AssetTrackerWebAPI.Controllers
{
[ApiController]
[Route("api/[controller]")]
 public class PlaidController: ControllerBase
    {
        private readonly PlaidService _plaidService;

        public PlaidController(PlaidService plaidService)
        {
            _plaidService = plaidService;
        }
        [HttpPost("createLinkToken")]
        public async Task<IActionResult> CreateLinkToken()
        {
            try
            {
                var token = await _plaidService.CreateLinkToken("test-user-123");
                return Ok(new { link_token = token });
            }
            catch (Exception ex)
            {
                // This will show Plaid's actual error in the response
                return BadRequest(new { error = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpPost("exchangePublicToken")]
        public async Task<IActionResult> ExchangePublicToken([FromBody] string publicToken)
        {
            try
            {
                await _plaidService.ExchangeAndStoreToken(publicToken, userId);
                return Ok(new { success = true });  
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}