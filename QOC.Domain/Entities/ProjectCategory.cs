namespace QOC.Domain.Entities
{
    public class ProjectCategory
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public string ImagePath { get; set; }
        public ICollection<Project.Project> Projects { get; set; }
    }
}
