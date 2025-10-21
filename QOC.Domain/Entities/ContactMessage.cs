using System.ComponentModel.DataAnnotations;

namespace QOC.Domain.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        
        [MaxLength(500)]
        public string? AttachmentPath { get; set; }
        
        [MaxLength(100)]
        public string? AttachmentOriginalName { get; set; }
        
        public DateTime SubmittedDate { get; set; }
        
        public bool IsRead { get; set; } = false;
        
        public DateTime? ReadDate { get; set; }
    }
}
