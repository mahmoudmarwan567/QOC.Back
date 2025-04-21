using QOC.Application.DTOs.Project;

namespace QOC.Application.DTOs.ProjectCategory
{
    public class ResponseProjectCategoryDto
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<ProjectResponseDto>? Projects { get; set; }
    }
}
