namespace QOC.Application.DTOs
{
    public class ServiceRequestDto
    {
        public string TitleAR { get; set; } = string.Empty;
        public string TitleEN { get; set; } = string.Empty;
        public string DescriptionAR { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
        public int Order { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
