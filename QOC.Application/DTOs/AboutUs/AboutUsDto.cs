namespace QOC.Application.DTOs
{
    public class AboutUsDto
    {
        public int Id { get; set; }
        public string DescriptionAR { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
        public string FullDescriptionAR { get; set; } = string.Empty;
        public string FullDescriptionEN { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
