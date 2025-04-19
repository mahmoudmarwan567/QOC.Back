using System.ComponentModel.DataAnnotations;

namespace QOC.Domain.Entities
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DescriptionAR { get; set; }
        [Required]
        public string DescriptionEN { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
