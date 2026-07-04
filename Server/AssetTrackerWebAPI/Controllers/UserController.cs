using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly UserService _userService;
        private readonly ProfileService _profileService;
        private readonly MockDataService _mockDataService;
        private readonly plaidItemService _plaiditemService;
        private readonly plaidService _plaidService;


        public UserController(UserService userService, ProfileService profileService, MockDataService mockDataService, plaidItemService plaiditemService, plaidService plaidService)
        {
            _userService = userService;
            _profileService = profileService;
            _mockDataService = mockDataService;
            _plaiditemService = plaiditemService;
            _plaidService = plaidService;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(string userId)
        {
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("CreateMockUser")]
        public async Task<ActionResult<User>> CreateMockUser()
        {
            var createdUser = await _mockDataService.CreateMockUser();
            return Ok(createdUser);
        }
        [HttpGet("GetUserId/{username}")]
        public async Task<ActionResult<string>> GetUserId(string username)
        {
        var userId = await _userService.GetUserId(username);
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("User not found"); 
        }
        return Ok(userId);
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            var profile = new Profile
            {
                profileId = Guid.NewGuid().ToString(),
                firstName = userRequest.firstName,
                lastName = userRequest.lastName
            };
            await _profileService.CreateProfile(profile);

            var user = new User
            {
                userId = Guid.NewGuid().ToString(),
                profileId = profile.profileId,
                email = userRequest.email,
                username = userRequest.username,
                password = userRequest.password
            };
        
            await _userService.CreateUser(user);
            return Ok();
        
        }
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            //get userId
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            //get profileId
            if(user.profileId == null)
            {
                return NotFound("Profile not found for this user");
            }
            await _userService.DeleteUser(userId);
            await _profileService.DeleteProfile(user.profileId);
            //remove all associated plaid items, accounts, and transactions
            var items = _plaiditemService.GetPlaidItems(user.profileId);
            if(items != null)
            {
                foreach(var item in items.Result)
                {
                    await _plaidService.RemoveItem(user.profileId, item.itemId);
                }
            }
            
            return Ok();
        }


}
}
