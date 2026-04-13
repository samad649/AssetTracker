using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController: ControllerBase
    {
       private readonly MockDataService _mockDataService;
       private readonly ProfileService _profileService;

       public ProfileController(MockDataService mockDataService, ProfileService profileService)
       {
        _mockDataService = mockDataService;
        _profileService = profileService;
        
       } 
         [HttpPost("CreateMockUser")]
        public async Task<ActionResult<User>> CreateMockUser()
        {
            var createdUser = await _mockDataService.CreateMockUser();
            return Ok(createdUser);
        }
        [HttpGet("{profileId}")]
        public async Task<ActionResult<Profile>> GetProfile(string profileId)
        {
            var profile = await _profileService.GetProfile(profileId);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Profile>>> GetAllProfiles()
        {
            var profiles = await _profileService.GetAllProfiles();
            return Ok(profiles);
        }
        [HttpGet("{profileId}/accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetProfileAccounts(string profileId)
        {
            var accounts = await _profileService.GetProfileAccounts(profileId);
            return Ok(accounts);
        }
        [HttpGet("accounts/{accountId}/transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAccountTransactions(string accountId)
        {
            var transactions = await _profileService.GetAccountTransactions(accountId);
            return Ok(transactions);
        }

    }
    
}
