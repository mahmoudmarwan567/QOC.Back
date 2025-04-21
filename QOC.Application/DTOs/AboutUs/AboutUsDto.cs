namespace QOC.Application.DTOs
{
    public class AboutUsDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
