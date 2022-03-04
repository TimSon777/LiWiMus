using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;

namespace LiWiMus.Core.Entities;

public class Playlist : BaseEntity
{
    public User Creator { get; set; }

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; }

    public bool IsPublic { get; set; }
    public string? PhotoPath { get; set; }

    public List<PlaylistTrack> Tracks { get; set; } = new();
    public List<LikedPlaylist> Subscribers { get; set; } = new();
}