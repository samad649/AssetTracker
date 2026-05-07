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
        private readonly MockDataService _mockDataService;


        public UserController(UserService userService, MockDataService mockDataService)
        {
            _userService = userService;
            _mockDataService = mockDataService;
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
    }
}