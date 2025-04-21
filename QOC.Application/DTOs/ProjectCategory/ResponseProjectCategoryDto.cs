using QOC.Application.DTOs.Project;

namespace QOC.Application.DTOs.ProjectCategory
{
    public class ResponseProjectCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<ProjectResponseDto>? Projects { get; set; }
    }
}
