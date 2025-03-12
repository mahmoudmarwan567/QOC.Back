using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.Role;
using QOC.Infrastructure.Services;

namespace QOC.Api.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
        {
            var result = await _roleService.CreateRoleAsync(request.Name);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Role created successfully" });
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
        {
            var result = await _roleService.UpdateRoleAsync(request.OldName, request.NewName);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Role updated successfully" });
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRoleAsync(roleName);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Role deleted successfully" });
        }
    }
}
