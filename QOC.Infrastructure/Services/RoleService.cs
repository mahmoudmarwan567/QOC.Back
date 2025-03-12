using Microsoft.AspNetCore.Identity;

namespace QOC.Infrastructure.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists" });

            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public List<string> GetAllRoles()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<IdentityResult> UpdateRoleAsync(string oldRole, string newRole)
        {
            var role = await _roleManager.FindByNameAsync(oldRole);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            role.Name = newRole;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            return await _roleManager.DeleteAsync(role);
        }
    }
}
