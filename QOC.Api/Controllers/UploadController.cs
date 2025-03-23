using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadSliderImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("لم يتم اختيار صورة");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets/images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}"; // اسم فريد
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { imageUrl = $"/assets/images/{fileName}" });
        }
    }
}
