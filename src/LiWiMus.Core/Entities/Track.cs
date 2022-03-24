using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities.Interfaces;

namespace LiWiMus.Core.Entities;

public class Track : BaseEntity, IMultipleArtistOwnersResource
{
    public List<ArtistTrack> ArtistTracks { get; set; } = new();
    public List<Artist> Artists { get; set; } = new();
    public Album Album { get; set; }
    public Genre Genre { get; set; }

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; }

    public DateOnly PublishedAt { get; set; }
    public string PathToFile { get; set; }

    public List<LikedSong> Subscribers { get; set; } = new();
    public List<PlaylistTrack> Playlists { get; set; } = new();
}