using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs;
using QOC.Infrastructure.Services;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyRequestDto dto)
        {
            var company = await _companyService.CreateCompanyAsync(dto);
            return Ok(company);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyRequestDto dto)
        {
            var updated = await _companyService.UpdateCompanyAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Company");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/Company/{uniqueFileName}";
            return Ok(new { imageUrl });
        }

        [HttpPost("upload-pdfs")]
        public async Task<IActionResult> UploadCompanyDocuments([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files received");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs/Company");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uploadedFiles = new List<object>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                // التحقق من نوع الملف
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".pdf")
                {
                    uploadedFiles.Add(new
                    {
                        fileName = file.FileName,
                        success = false,
                        error = "Only PDF files are allowed"
                    });
                    continue;
                }

                // التحقق من حجم الملف (حد أقصى 10 ميجا)
                if (file.Length > 10 * 1024 * 1024)
                {
                    uploadedFiles.Add(new
                    {
                        fileName = file.FileName,
                        success = false,
                        error = "File size cannot exceed 10MB"
                    });
                    continue;
                }

                var uniqueFileName = Guid.NewGuid().ToString() + ".pdf";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var pdfUrl = $"/pdfs/Company/{uniqueFileName}";
                uploadedFiles.Add(new
                {
                    fileName = file.FileName,
                    success = true,
                    pdfUrl = pdfUrl
                });
            }

            return Ok(new { files = uploadedFiles });
        }
    }
}
