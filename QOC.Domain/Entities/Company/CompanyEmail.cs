using System.Text.Json.Serialization;

namespace QOC.Domain.Entities
{
    public class CompanyEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
