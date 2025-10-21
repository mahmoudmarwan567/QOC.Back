using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IWebHostEnvironment _environment;

        public ContactController(
            IContactService contactService,
            IWebHostEnvironment environment)
        {
            _contactService = contactService;
            _environment = environment;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitContactForm([FromForm] ContactSubmissionDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string? attachmentPath = null;
                string? originalFileName = null;

                // ?????? ????? ?????? ??? ??? ???????
                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    // ?????? ?? ??? ????? (5MB)
                    if (model.Attachment.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest(new { message = "File size exceeds 5MB limit" });
                    }

                    // ?????? ?? ??? ?????
                    var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(model.Attachment.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { message = "File type not allowed. Allowed types: PDF, Word, Images" });
                    }

                    // ??? ?????
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "contact");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Attachment.CopyToAsync(fileStream);
                    }

                    attachmentPath = $"/uploads/contact/{uniqueFileName}";
                    originalFileName = model.Attachment.FileName;
                }

                // ??? ???????? ?? ????? ????????
                var contactMessage = new ContactMessage
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    AttachmentPath = attachmentPath,
                    AttachmentOriginalName = originalFileName
                };

                var result = await _contactService.AddAsync(contactMessage);

                return Ok(new
                {
                    success = true,
                    message = "Your message has been sent successfully",
                    messageAR = "?? ????? ?????? ?????",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var messages = await _contactService.GetAllAsync(page, pageSize);
                return Ok(new
                {
                    success = true,
                    data = messages,
                    page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            try
            {
                var message = await _contactService.GetByIdAsync(id);
                if (message == null)
                    return NotFound(new { success = false, message = "Message not found" });

                return Ok(new { success = true, data = message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                await _contactService.MarkAsReadAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Message marked as read",
                    messageAR = "?? ??? ????? ????? ??? ???????"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Message deleted successfully",
                    messageAR = "?? ??? ??????? ?????"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var count = await _contactService.GetUnreadCountAsync();
                return Ok(new { success = true, count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
