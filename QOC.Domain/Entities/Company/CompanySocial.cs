namespace QOC.Domain.Entities
{
    public class CompanySocial
    {
        public int Id { get; set; }
        public string SocialNameAR { get; set; }
        public string SocialNameEN { get; set; }
        public string SocialIconPath { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
