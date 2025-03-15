using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.User;
using QOC.Infrastructure.Services;

namespace QOC.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request.FullName, request.Email, request.Password, request.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _userService.UpdateUserAsync(id, request);

            if (!success)
                return BadRequest(new { Error = message });

            return Ok(new { Message = message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User deleted successfully" });
        }
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UpdateUserRoleRequest request)
        {
            var result = await _userService.UpdateUserRoleAsync(id, request.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User role updated successfully" });
        }
    }
}
