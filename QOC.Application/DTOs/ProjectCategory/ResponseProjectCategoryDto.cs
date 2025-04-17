namespace QOC.Application.DTOs.ProjectCategory
{
    public class ResponseProjectCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ResponseSingleProjectDto? Project { get; set; }
    }
}
