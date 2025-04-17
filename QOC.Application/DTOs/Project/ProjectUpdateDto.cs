namespace QOC.Application.DTOs.Project
{
    public class ProjectUpdateDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectProperties { get; set; }
        public int ProjectCategoryId { get; set; }
        public List<ProjectImageUpdateDto> ProjectImages { get; set; } = new();
    }
}
