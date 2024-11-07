using Microsoft.AspNetCore.Mvc;
using RoomReservationBackend.DTOs;
using RoomReservationBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace RoomReservationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;

        public UsersController(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(CreateUserDto createUserDto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(createUserDto);
                if (result == null)
                {
                    return BadRequest("User with this email already exists.");
                }

                var token = _authService.GenerateToken(result);
                return Ok(new { user = result, token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _userService.AuthenticateAsync(loginDto);
                if (result == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var token = _authService.GenerateToken(result);
                return Ok(new { user = result, token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while logging in.");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _userService.GetUserByIdAsync(int.Parse(userId));
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the user profile.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, updateUserDto);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
    }
}