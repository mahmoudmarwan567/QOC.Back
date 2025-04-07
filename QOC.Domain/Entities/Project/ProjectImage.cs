namespace QOC.Domain.Entities.Project
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
    }
}
