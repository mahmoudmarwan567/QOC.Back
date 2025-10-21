namespace QOC.Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Logo { get; set; }
        public ICollection<CompanyAddress> Addresses { get; set; }
            = new List<CompanyAddress>();
        public ICollection<CompanyPhone> Phones { get; set; }
            = new List<CompanyPhone>();
        public ICollection<CompanyEmail> Emails { get; set; }
            = new List<CompanyEmail>();
        public List<CompanySocial> CompanySocials { get; set; }
            = new List<CompanySocial>();
        public ICollection<CompanyDocument> Documents { get; set; }
            = new List<CompanyDocument>();
    }
}

