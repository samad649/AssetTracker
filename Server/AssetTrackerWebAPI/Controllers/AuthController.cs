using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;

namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
        public async Task<ActionResult<string>> Login([FromBody] User loginRequest)
        {
            if (loginRequest.email == null || loginRequest.password == null)
                return BadRequest("Email and password are required");
                
            var user = await _authService.Validate(loginRequest.email, loginRequest.password);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {email}", loginRequest.email);
                return Unauthorized("Invalid credentials");
            }

            var token = _authService.GenerateToken(user);
            _logger.LogInformation("Login successful for {email}", user.email);

            return Ok(new { token });
        }
    }
}