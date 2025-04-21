namespace QOC.Application.DTOs
{
    public class SliderDto
    {
        public int Id { get; set; }
        public string TitleAR { get; set; } = string.Empty;
        public string TitleEN { get; set; } = string.Empty;
        public string SubtitleAR { get; set; } = string.Empty;
        public string SubtitleEN { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string DescriptionAR { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
        public string ButtonTextAR { get; set; } = string.Empty;
        public string ButtonTextEN { get; set; } = string.Empty;
        public string ButtonLink { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
