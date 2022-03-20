using System.Reflection;
using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Entities;
using LiWiMus.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges() {
        return this.SaveChangesWithTriggers(base.SaveChanges, true);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess) {
        return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, true, cancellationToken);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
        return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
    }
}