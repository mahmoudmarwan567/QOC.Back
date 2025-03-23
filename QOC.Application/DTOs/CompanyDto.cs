using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOC.Application.DTOs
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public List<string> Addresses { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
    }

}
