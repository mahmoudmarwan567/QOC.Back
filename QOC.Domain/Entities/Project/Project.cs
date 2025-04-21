using System.ComponentModel.DataAnnotations;

namespace QOC.Domain.Entities.Project
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProjectNameAR { get; set; }
        [Required]
        public string ProjectNameEN { get; set; }
        [Required]
        public string ProjectDescriptionAR { get; set; }
        public string ProjectDescriptionEN { get; set; }
        public string ProjectPropertiesAR { get; set; }
        public string ProjectPropertiesEN { get; set; }
        public int ProjectCategoryId { get; set; }
        public virtual ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();
    }
}
