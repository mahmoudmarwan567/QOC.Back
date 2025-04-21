namespace QOC.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string ProjectNameAR { get; set; }
        public string ProjectNameEN { get; set; }
        public string ProjectDescriptionAR { get; set; }
        public string ProjectDescriptionEN { get; set; }
        public string ProjectPropertiesAR { get; set; }
        public string ProjectPropertiesEN { get; set; }
        public int ProjectCategoryId { get; set; }
        public List<CreateProjectImageDto> ProjectImages { get; set; }
    }
}
