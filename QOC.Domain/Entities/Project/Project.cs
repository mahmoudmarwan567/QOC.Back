using System.ComponentModel.DataAnnotations;

namespace QOC.Domain.Entities.Project
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        public string ProjectProperties { get; set; }
        public int ProjectCategoryId { get; set; }
        public virtual ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();
    }
}
