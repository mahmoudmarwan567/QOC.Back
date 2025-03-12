using Microsoft.AspNetCore.Identity;

namespace QOC.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
