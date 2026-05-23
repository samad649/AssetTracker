using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController: ControllerBase
    {
        private readonly accountService _accountService;
        private readonly ProfileService _profileService;
        public AccountController(accountService accountService, ProfileService profileService)
        {
            _accountService = accountService;
            _profileService = profileService;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var userId = User.FindFirst("userId")?.Value; 
            if (userId == null) return Unauthorized();

            var profile = await _profileService.GetProfileByUserId(userId);
            if (profile == null) return NotFound("Profile not found");

            var accounts = await _accountService.GetAllAccounts(profile.profileId);
            return Ok(accounts ?? new List<Account>()); // ← always return 200 with list (empty or not)
        }
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(string accountId)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();
            var profile = await _profileService.GetProfileByUserId(userId);
            if(profile == null) return NotFound("Profile not found");
            var account = await _accountService.GetAccount(profile.profileId, accountId);
            if (account == null) return NotFound("Account not found");
            return Ok(account);
        }
        [HttpGet("ByItem/{itemId}")]
        public async Task<IActionResult> GetAccountsByItemId(string itemId)
        {
            var accounts = await _accountService.GetAccountsByItemId(itemId);
            if (accounts == null || !accounts.Any()) return NotFound("No accounts found for this item");
            return Ok(accounts);
        }
    }
}