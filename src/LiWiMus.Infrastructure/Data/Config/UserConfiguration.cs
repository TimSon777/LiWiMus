using LiWiMus.Core.IdentityAggregates;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Gender)
               .HasConversion<string>();

        builder.HasOne(u => u.IdentityAggregate)
               .WithOne(ia => ia.User)
               .HasForeignKey<User>(u => u.IdentityAggregateId);
    }
}