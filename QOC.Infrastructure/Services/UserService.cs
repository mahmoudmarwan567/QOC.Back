using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QOC.Application.DTOs.User;
using QOC.Domain.Entities;

namespace QOC.Infrastructure.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return userList;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }

        public async Task<IdentityResult> CreateUserAsync(string fullName, string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("full name cannot be null or empty", nameof(fullName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be null or empty", nameof(role));

            var existingUser = await _userManager.FindByNameAsync(fullName);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username is already taken" });

            existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use" });

            var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogError("User creation failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return result;
            }

            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null && existingUser.Id != userId)
                return (false, "Username is already taken");

            existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != userId)
                return (false, "Email is already in use");

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.FullName = request.FullName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to update user details: {Errors}", string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return (false, "Failed to update user details");
            }

            if (!string.IsNullOrWhiteSpace(request.OldPassword) &&
                !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
                if (!passwordCheck)
                    return (false, "Old password is incorrect");

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    _logger.LogError("Failed to change password: {Errors}", string.Join(", ", passwordChangeResult.Errors.Select(e => e.Description)));
                    return (false, string.Join(", ", passwordChangeResult.Errors.Select(e => e.Description)));
                }
            }

            return (true, "User updated successfully");
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserRoleAsync(string userId, string newRole)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            if (string.IsNullOrWhiteSpace(newRole))
                throw new ArgumentException("Role cannot be null or empty", nameof(newRole));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);  // Remove old roles

            if (await _roleManager.RoleExistsAsync(newRole))
            {
                return await _userManager.AddToRoleAsync(user, newRole);  // Add new role
            }

            return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
        }
    }
}
