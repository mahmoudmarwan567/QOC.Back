namespace QOC.Application.DTOs.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string ProjectNameAR { get; set; }
        public string ProjectNameEN { get; set; }
        public string ProjectDescriptionAR { get; set; }
        public string ProjectDescriptionEN { get; set; }
        public string ProjectPropertiesAR { get; set; }
        public string ProjectPropertiesEN { get; set; }
        public int ProjectCategoryId { get; set; }
        public List<ProjectImageResponseDto> ProjectImages { get; set; }
    }
}
