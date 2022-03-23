using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
         .WithOne(e => e.Role)
         .HasForeignKey(ur => ur.RoleId)
         .IsRequired();
    }
}