using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.Auth;
using QOC.Infrastructure.Services;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(new { message = result });
        }

        // Login Endpoint
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
    }
}
