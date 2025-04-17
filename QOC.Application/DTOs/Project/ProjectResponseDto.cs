namespace QOC.Application.DTOs.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectProperties { get; set; }
        public int ProjectCategoryId { get; set; }
        public List<ProjectImageResponseDto> ProjectImages { get; set; }
    }
}
