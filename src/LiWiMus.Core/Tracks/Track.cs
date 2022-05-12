using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Genres;
using LiWiMus.Core.LikedSongs;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Shared.Interfaces;

namespace LiWiMus.Core.Tracks;

public class Track : BaseEntity, IResource.WithMultipleOwners<Artist>
{
    public virtual ICollection<Artist> Owners { get; set; } = null!;
    public virtual ICollection<Genre> Genres { get; set; } = null!;

    public virtual Album Album { get; set; } = null!;
    
    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    public DateOnly PublishedAt { get; set; }
    public string FileLocation { get; set; } = null!;
    public double Duration { get; set; }

    public virtual List<LikedSong> Subscribers { get; set; } = null!;
    public virtual List<PlaylistTrack> Playlists { get; set; } = null!;
}