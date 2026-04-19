using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        [HttpPost("createLinkToken")]
        public async Task<IActionResult> CreateLinkToken([FromBody] CreateLinkTokenRequest request)
        {
            Console.WriteLine("=== CreateLinkToken controller hit ===");
            try
            {
                var token = await _plaidService.CreateLinkToken(request.UserId);
                return Ok(new { link_token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== Exception: {ex.Message} ===");
                return BadRequest(new { error = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpPost("exchangePublicToken")]
        public async Task<IActionResult> ExchangePublicToken([FromBody] ExchangeTokenRequest request)
        {
            try
            {
                await _plaidService.ExchangeAndStoreToken(request.PublicToken, request.UserId);
                return Ok(new { success = true });  
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}