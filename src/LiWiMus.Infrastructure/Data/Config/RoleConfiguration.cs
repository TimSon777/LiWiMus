using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LiWiMus.Infrastructure.Data.Config;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(role => role.DefaultTimeout)
               .HasConversion(new TimeSpanToStringConverter())
               .HasDefaultValue(TimeSpan.MaxValue);

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
         .WithOne(e => e.Role)
         .HasForeignKey(ur => ur.RoleId)
         .IsRequired();
    }
}