using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using BCrypt.Net;
using System.Security.Claims;

namespace Cleopatra.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SalonContext _context;

        public AuthController(SalonContext context)
        {
            _context = context;
        }

        // POST: /auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.Employees.FirstOrDefault(u =>
                (u.username == loginRequest.Username || u.email == loginRequest.Username) &&
                u.isDeleted == false);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.password_hash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Creating user identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username ?? user.email),
                new Claim(ClaimTypes.Role, user.role),
                new Claim("employee_id", user.employee_id.ToString()) // Dodanie employee_id jako Claim
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Logging in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Returning user details
            return Ok(new
            {
                id = user.employee_id,
                username = user.username,
                role = user.role
            });
        }


        // POST: /auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }

        // GET: /auth/session
        [HttpGet("session")]
        public IActionResult GetSession()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("No active session.");
            }

            return Ok(new
            {
                username = User.Identity.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
