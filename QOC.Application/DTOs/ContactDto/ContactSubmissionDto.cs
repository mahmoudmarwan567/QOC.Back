using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace QOC.Application.DTOs
{
    public class ContactSubmissionDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Message { get; set; }

        public IFormFile? Attachment { get; set; }
    }
}
