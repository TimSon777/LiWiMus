using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Gender)
               .HasConversion<string>();

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
         .WithOne()
         .HasForeignKey(uc => uc.UserId)
         .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
         .WithOne()
         .HasForeignKey(ul => ul.UserId)
         .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
         .WithOne()
         .HasForeignKey(ut => ut.UserId)
         .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
         .WithOne(e => e.User)
         .HasForeignKey(ur => ur.UserId)
         .IsRequired();
    }
}