using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities.Interfaces;

namespace LiWiMus.Core.Entities;

public class Playlist : BaseEntity, ISingleOwnerResource
{
    public User User { get; set; }
    public int UserId { get; set; }

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    public bool IsPublic { get; set; }
    public string? PhotoPath { get; set; }

    public List<PlaylistTrack> Tracks { get; set; } = new();
    public List<LikedPlaylist> Subscribers { get; set; } = new();
}