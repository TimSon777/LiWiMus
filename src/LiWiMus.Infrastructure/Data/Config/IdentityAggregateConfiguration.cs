using LiWiMus.Core.IdentityAggregates;
using LiWiMus.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class IdentityAggregateConfiguration : IEntityTypeConfiguration<IdentityAggregate>
{
    public void Configure(EntityTypeBuilder<IdentityAggregate> builder)
    {
        builder
            .HasKey(ia => ia.IdentityId);

        builder
            .HasOne<UserIdentity>()
            .WithOne(identity => identity.Aggregate)
            .HasForeignKey<IdentityAggregate>(ia => ia.IdentityId);
    }
}