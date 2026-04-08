using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AssetTrackerWebAPI.Controllers
{
    class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("api/auth/login")]
        public ActionResult<string> Login([FromBody] User loginRequest)
        {
            // In a real application, you would validate the user's credentials here
            // For this example, we'll just create a token for any user

            var token = _authService.GenerateToken(loginRequest);
            return Ok(token);
        }
    }
}