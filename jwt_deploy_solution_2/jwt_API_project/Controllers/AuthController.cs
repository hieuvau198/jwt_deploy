using jwt_Application_project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace jwt_API_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly Dictionary<string, string> RefreshTokens = new();
        private readonly JwtHelper _jwtHelper;

        public AuthController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "test" && request.Password == "password") // Simple validation
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, request.Username)
            };

                var accessToken = _jwtHelper.GenerateAccessToken(claims);
                var refreshToken = _jwtHelper.GenerateRefreshToken();

                RefreshTokens[refreshToken] = request.Username;

                return Ok(new { accessToken, refreshToken });
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            if (RefreshTokens.TryGetValue(request.RefreshToken, out var username))
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, username)
            };

                var accessToken = _jwtHelper.GenerateAccessToken(claims);
                var newRefreshToken = _jwtHelper.GenerateRefreshToken();

                RefreshTokens.Remove(request.RefreshToken);
                RefreshTokens[newRefreshToken] = username;

                return Ok(new { accessToken, refreshToken = newRefreshToken });
            }

            return Unauthorized("Invalid refresh token");
        }
    }

    public record LoginRequest(string Username, string Password);
    public record RefreshRequest(string RefreshToken);
}
