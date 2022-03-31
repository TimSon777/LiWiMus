using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.HasMany(p => p.Owners)
               .WithMany(p => p.Albums)
               .UsingEntity<ArtistAlbum>(
                   j => j
                        .HasOne(pt => pt.Artist)
                        .WithMany(t => t.ArtistAlbums)
                        .HasForeignKey(pt => pt.ArtistId),
                   j => j
                        .HasOne(pt => pt.Album)
                        .WithMany(p => p.ArtistAlbums)
                        .HasForeignKey(pt => pt.AlbumId));

    }
}