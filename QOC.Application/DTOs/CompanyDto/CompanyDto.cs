﻿namespace QOC.Application.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string LogoPath { get; set; }
        public List<string> AddressesAR { get; set; }
        public List<string> AddressesEN { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
        public List<CompanySocialDto> CompanySocials { get; set; }
    }

}
