using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QOC.Domain.Entities
{
    public class CompanyAddress
    {
        public int Id { get; set; }
        public string Address { get; set; }

        // الربط مع الشركة
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
