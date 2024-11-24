using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  // Add this namespace for AllowAnonymous
using RoleJwtApi1.Models;
using JWT_Authentication_Authorization.Services;
using RoleJwtApi1.Service;

namespace RoleJwtApi1.Controllers
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

        // Register endpoint (No authentication required)
        [HttpPost("register")]
        [AllowAnonymous]  // This allows unauthenticated access
        public ActionResult<User> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Username = request.Username,  // Access Username from RegisterRequest
                Password = request.Password
            };

            var registeredUser = _authService.AddUser(user);
            return Ok(registeredUser);
        }

        // Login endpoint (No authentication required)
        [HttpPost("login")]
        [AllowAnonymous]  // This allows unauthenticated access
        public ActionResult<string> Login([FromBody] LoginRequest loginRequest)
        {
            var token = _authService.Login(loginRequest);
            return Ok(token);
        }
    }
}
