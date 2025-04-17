using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.ProjectCategory;
using QOC.Infrastructure.Contracts;

namespace QOC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectCategoryController : ControllerBase
    {
        private readonly IProjectCategoryService _service;

        public ProjectCategoryController(IProjectCategoryService service)
        {
            _service = service;
        }

        // GET: api/ProjectCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseProjectCategoryDto>>> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/ProjectCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseProjectCategoryDto>> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/ProjectCategory
        [HttpPost]
        public async Task<ActionResult<ResponseProjectCategoryDto>> Create(RequestProjectCategoryDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/ProjectCategory/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseProjectCategoryDto>> Update(int id, RequestProjectCategoryDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return result switch
            {
                "Category not found." => NotFound(new { message = result }),
                "Cannot delete. There are related projects." => BadRequest(new { message = result }),
                "Category deleted successfully." => Ok(new { message = result }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }
        [HttpGet("basic")]
        public async Task<ActionResult<IEnumerable<ResponseBasicProjectCategoryDto>>> GetBasic()
        {
            var categories = await _service.GetAllWithoutProjectsAsync();
            return Ok(categories);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProjectCategory");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/ProjectCategory/{uniqueFileName}";
            return Ok(new { imageUrl });
        }
    }
}
