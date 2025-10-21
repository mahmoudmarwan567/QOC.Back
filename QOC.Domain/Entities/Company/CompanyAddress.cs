using System.Text.Json.Serialization;

namespace QOC.Domain.Entities
{
    public class CompanyAddress
    {
        public int Id { get; set; }
        public string AddressAR { get; set; }
        public string AddressEN { get; set; }
        public string? MapLink { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
