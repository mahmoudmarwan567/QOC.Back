namespace QOC.Application.DTOs
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public List<string> Addresses { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
        public List<CompanySocialDto> CompanySocials { get; set; }
    }

}
