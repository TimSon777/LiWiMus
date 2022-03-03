using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class LikedUserConfiguration : IEntityTypeConfiguration<LikedUser>
{
    public void Configure(EntityTypeBuilder<LikedUser> builder)
    {
        builder.HasOne(lu => lu.User)
               .WithMany(u => u.Subscribers);

        builder.HasOne(lu => lu.Liked)
               .WithMany(u => u.LikedUsers);
    }
}