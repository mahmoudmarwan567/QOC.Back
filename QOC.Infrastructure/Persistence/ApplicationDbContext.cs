using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QOC.Domain.Entities;
using QOC.Domain.Entities.Project;

namespace QOC.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public DbSet<CompanyPhone> CompanyPhones { get; set; }
        public DbSet<CompanyEmail> CompanyEmails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<CompanySocial> CompanySocials { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Project>()
                    .HasMany(p => p.ProjectImages)
                    .WithOne(pi => pi.Project)
                    .HasForeignKey(pi => pi.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
