using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.Project;
using QOC.Infrastructure.Contracts;

namespace QOC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateProjectDto>>> GetAllProjects()
        {
            return Ok(await _projectService.GetAllProjectsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreateProjectDto>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromForm] CreateProjectDto projectDto)
        {
            var result = await _projectService.CreateProjectAsync(projectDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> UpdateProject(int id, [FromForm] ProjectUpdateDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest();
            }

            var updatedProject = await _projectService.UpdateProjectAsync(projectDto);
            if (updatedProject == null)
            {
                return NotFound();
            }

            return Ok(updatedProject);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var deleted = await _projectService.DeleteProjectAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
