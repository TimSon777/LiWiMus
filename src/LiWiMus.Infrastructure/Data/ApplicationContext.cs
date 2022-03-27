using System.Reflection;
using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Entities;
using LiWiMus.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<User, Role, int, UserClaim, UserRole,
    IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    private readonly IServiceProvider _serviceProvider;
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options, IServiceProvider serviceProvider) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        return this.SaveChangesWithTriggers(base.SaveChanges, _serviceProvider, true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return this.SaveChangesWithTriggers(base.SaveChanges, _serviceProvider, acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, _serviceProvider, true, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                               CancellationToken cancellationToken = default)
    {
        return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, _serviceProvider, acceptAllChangesOnSuccess, cancellationToken);
    }
}