namespace QOC.Application.DTOs
{
    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string? AttachmentPath { get; set; }
        public string? AttachmentOriginalName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
