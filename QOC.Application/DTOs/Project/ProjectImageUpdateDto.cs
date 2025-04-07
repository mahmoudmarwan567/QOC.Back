using Microsoft.AspNetCore.Http;

namespace QOC.Application.DTOs.Project
{
    public class ProjectImageUpdateDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }
    }
}
