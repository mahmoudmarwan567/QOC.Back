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
        public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            var result = await _projectService.CreateProjectAsync(projectDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> UpdateProject(int id, [FromBody] ProjectUpdateDto projectDto)
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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files received.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Project");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var imageUrl = $"/images/Project/{uniqueFileName}";
                    uploadedUrls.Add(imageUrl);
                }
            }

            return Ok(new { imageUrls = uploadedUrls });
        }
        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjectsByCategoryPaged(
          int categoryId,
          int page = 1,
          int pageSize = 9)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            // استدعاء الدالة الجديدة اللي تدعم Pagination في الـ service
            var projects = await _projectService.GetProjectsByCategoryPagedAsync(categoryId, page, pageSize);

            return Ok(projects);
        }

    }
}
