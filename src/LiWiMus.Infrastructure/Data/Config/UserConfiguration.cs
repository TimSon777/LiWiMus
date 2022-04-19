using LiWiMus.Core.Plans;
using LiWiMus.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Gender)
               .HasConversion<string>();

        builder.HasOne(u => u.UserPlan)
               .WithOne(up => up.User)
               .HasForeignKey<UserPlan>(u => u.UserId);
    }
}