using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QOC.Domain.Entities;

namespace QOC.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Service> Services { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public DbSet<CompanyPhone> CompanyPhones { get; set; }
        public DbSet<CompanyEmail> CompanyEmails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }



    }
}
