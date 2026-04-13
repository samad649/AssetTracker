using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly UserService _userService;


        public UserController(UserService userService)
        {
            _userService = userService;
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
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
    }
}