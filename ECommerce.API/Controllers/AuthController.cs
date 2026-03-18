using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            var result = await _authService.RegisterAsync(registerUserDTO);
            return CreatedAtAction(nameof(Register), result);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);

            // Create a secure cookie options
            var cookieOptions = new CookieOptions
            { 
                HttpOnly = true, // js can't read this
                Secure = true, // Require https
                SameSite = SameSiteMode.Strict, // no csrf
                Expires = DateTime.UtcNow.AddDays(7),
            };

            Response.Cookies.Append("accessToken", result.AccessToken, cookieOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);


            return Ok(new { User = result.User});
        }

        // POST: api/Auth/refresh-token
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDTO);
            return Ok(result);
        }


        // POST : api/Auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            await _authService.LogoutAsync(refreshTokenDTO);
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }


        // PUT: api/Auth/change-password
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            // Get userId from JWT token claims - don't trust body for this
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim is null)
                return Unauthorized();

            changePasswordDTO.UserId = Guid.Parse(userIdClaim);
            await _authService.ChangePasswordAsync(changePasswordDTO);
            return NoContent();
        }

        // GET: api/Auth/me
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (userId is null) return Unauthorized();

            return Ok(new
            {
                UserId = Guid.Parse(userId),
                UserName = userName,
                Email = email,
                Role = role
            });
        }
    }
}
