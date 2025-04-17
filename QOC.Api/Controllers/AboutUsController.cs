using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs;
using QOC.Application.Interfaces;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var aboutUs = await _aboutUsService.GetAllAsync();
            return Ok(aboutUs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aboutUs = await _aboutUsService.GetByIdAsync(id);
            if (aboutUs == null) return NotFound();
            return Ok(aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AboutUsDto dto)
        {
            var created = await _aboutUsService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AboutUsDto dto)
        {
            var updated = await _aboutUsService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _aboutUsService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/AboutUs");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{uniqueFileName}"; // To be served as static file
            return Ok(new { imageUrl });
        }



    }
}
