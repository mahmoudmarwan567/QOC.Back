namespace QOC.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<CreateProjectImageDto> ProjectImages { get; set; }
    }
}
