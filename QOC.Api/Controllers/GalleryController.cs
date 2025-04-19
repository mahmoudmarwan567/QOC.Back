using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs.Gallery;
using QOC.Infrastructure.Contracts;

namespace QOC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryDto>>> GetAll()
        {
            var galleries = await _galleryService.GetAllAsync();
            return Ok(galleries);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<GalleryDto>>> BulkCreate([FromBody] IEnumerable<GalleryRequestDto> galleries)
        {
            if (galleries == null || !galleries.Any())
                return BadRequest("Gallery list is empty or null.");

            var created = await _galleryService.BulkCreateAsync(galleries);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _galleryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Gallery");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/Gallery/{uniqueFileName}"; // To be served as static file
            return Ok(new { imageUrl });
        }
    }
}
