using System.Reflection;
using LiWiMus.Core.Entities;
using LiWiMus.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Infrastructure.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<User> Users => Set<User>();

    public ApplicationContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
                      .Entries()
                      .Where(e => e.Entity is BaseEntity &&
                                  e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity) entityEntry.Entity).CreatedAt = DateTime.Now;
            }
            else
            {
                Entry((BaseEntity) entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
            }

            ((BaseEntity) entityEntry.Entity).ModifiedAt = DateTime.Now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}