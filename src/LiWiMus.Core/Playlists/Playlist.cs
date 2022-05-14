using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Shared.Interfaces;
using LiWiMus.Core.Tracks;

namespace LiWiMus.Core.Playlists;

public class Playlist : BaseEntity, IResource.WithOwner<User>
{
    public virtual User Owner { get; set; } = null!;
    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    public bool IsPublic { get; set; }
    public string? PhotoLocation { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = null!;
    public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = null!;
    public virtual ICollection<LikedPlaylist> Subscribers { get; set; } = null!;
}