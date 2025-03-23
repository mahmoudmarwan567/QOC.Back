using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOC.Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // يمكن أن يكون Logo عبارة عن مسار لملف أو Base64 أو ما يناسبك
        public string Logo { get; set; }

        // العناوين
        public ICollection<CompanyAddress> Addresses { get; set; }
            = new List<CompanyAddress>();

        // أرقام الهواتف
        public ICollection<CompanyPhone> Phones { get; set; }
            = new List<CompanyPhone>();

        // الإيميلات
        public ICollection<CompanyEmail> Emails { get; set; }
            = new List<CompanyEmail>();
    }
}

