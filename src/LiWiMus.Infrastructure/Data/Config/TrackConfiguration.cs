using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasMany(p => p.Artists)
               .WithMany(p => p.Tracks)
               .UsingEntity<ArtistTrack>(
                   j => j
                        .HasOne(pt => pt.Artist)
                        .WithMany(t => t.ArtistTracks)
                        .HasForeignKey(pt => pt.ArtistId),
                   j => j
                        .HasOne(pt => pt.Track)
                        .WithMany(p => p.ArtistTracks)
                        .HasForeignKey(pt => pt.TrackId));
    }
}