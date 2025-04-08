public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService, IDisposable, IAsyncDisposable
{
    public DbSet<Service> Services { get; set; } // Add this line

    // Other DbSet properties...
}
