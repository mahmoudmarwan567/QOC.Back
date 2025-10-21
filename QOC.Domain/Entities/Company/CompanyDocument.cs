using System.Text.Json.Serialization;

namespace QOC.Domain.Entities
{
    public class CompanyDocument
    {
        public int Id { get; set; }
        public string TitleAR { get; set; }
        public string TitleEN { get; set; }
        public string FilePath { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
