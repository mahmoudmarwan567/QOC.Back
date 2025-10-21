namespace QOC.Application.DTOs
{
    public class CompanyDocumentDto
    {
        public int Id { get; set; }
        public string TitleAR { get; set; }
        public string TitleEN { get; set; }
        public string FilePath { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
