// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            if (await _userService.GetByEmailAsync(request.Email) != null)
                return Conflict("User already exists");

            var user = new User
            {
                Email = request.Email.ToLowerInvariant(),
                FullName = request.FullName,
                PasswordHash = request.Password // will be hashed inside CreateAsync
            };

            var createdUser = await _userService.CreateAsync(user);
            var token = _jwtService.GenerateToken(createdUser);

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = createdUser.Id!,
                Email = createdUser.Email,
                FullName = createdUser.FullName,
                Role = createdUser.Role
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email.ToLowerInvariant());
            if (user == null || !await _userService.ValidateUserAsync(request.Email, request.Password))
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.Id!,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            });
        }
    }
}