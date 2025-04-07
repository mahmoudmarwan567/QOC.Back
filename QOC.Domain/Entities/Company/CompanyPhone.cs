using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
