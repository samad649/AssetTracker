using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;

namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Username == null || loginRequest.Password == null)
                return BadRequest("Username and Password are required");

            var user = await _authService.Validate(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {username}", loginRequest.Username);
                return Unauthorized("Invalid credentials");
            }

            var token = _authService.GenerateToken(user);
            _logger.LogInformation("Login successful for {username}", user.username);

            return Ok(new { token, user });

        }
    }
}