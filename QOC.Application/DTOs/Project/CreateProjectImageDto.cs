using Microsoft.AspNetCore.Http;

namespace QOC.Application.DTOs.Project
{
    public class CreateProjectImageDto
    {
        public IFormFile ImageFile { get; set; }
    }
}
