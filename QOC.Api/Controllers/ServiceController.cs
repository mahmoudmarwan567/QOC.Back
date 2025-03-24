using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs;
using QOC.Application.Interfaces;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceDto dto)
        {
            var created = await _serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceDto dto)
        {
            var updated = await _serviceService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceService.DeleteAsync(id);
            return NoContent();
        }
    }

}
