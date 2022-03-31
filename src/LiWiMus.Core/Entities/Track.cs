using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities.Interfaces;

namespace LiWiMus.Core.Entities;

public class Track : BaseEntity, IResource.WithMultipleOwners<Artist>
{
    public List<Artist> Owners { get; set; } = new();
    public List<ArtistTrack> ArtistTracks { get; set; } = new();
    public Album Album { get; set; } = null!;
    public Genre Genre { get; set; } = null!;

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    public DateOnly PublishedAt { get; set; }
    public string PathToFile { get; set; } = null!;

    public List<LikedSong> Subscribers { get; set; } = new();
    public List<PlaylistTrack> Playlists { get; set; } = new();
}