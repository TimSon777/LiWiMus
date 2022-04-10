using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Shared.Interfaces;

namespace LiWiMus.Core.Playlists;

public class Playlist : BaseEntity, IResource.WithOwner<User>
{
    public User Owner { get; set; } = null!;
    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    public bool IsPublic { get; set; }
    public string? PhotoPath { get; set; }

    public List<PlaylistTrack> Tracks { get; set; } = new();
    public List<LikedPlaylist> Subscribers { get; set; } = new();
}