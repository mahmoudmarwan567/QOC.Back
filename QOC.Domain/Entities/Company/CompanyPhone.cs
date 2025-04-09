using System.Text.Json.Serialization;

namespace QOC.Domain.Entities
{
    public class CompanyPhone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
