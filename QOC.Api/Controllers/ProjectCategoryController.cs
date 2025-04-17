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

        // DELETE: api/ProjectCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Deleted Successfully");
        }
    }
}
