using System.Text.Json.Serialization;

namespace QOC.Domain.Entities
{
    public class CompanyAddress
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
