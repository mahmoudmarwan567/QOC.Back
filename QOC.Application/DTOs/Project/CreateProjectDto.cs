namespace QOC.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int ProjectCategoryId { get; set; }
        public string ProjectProperties { get; set; }
        public List<CreateProjectImageDto> ProjectImages { get; set; }
    }
}
