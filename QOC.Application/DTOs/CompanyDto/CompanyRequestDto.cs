namespace QOC.Application.DTOs
{
    public class CompanyRequestDto
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string LogoPath { get; set; }
        public List<CompanyAddressDto> Addresses { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
        public List<CompanySocialRequestDto> CompanySocials { get; set; }
        public List<CompanyDocumentRequestDto> Documents { get; set; }
    }
}
