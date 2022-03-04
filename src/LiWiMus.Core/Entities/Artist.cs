using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;

namespace LiWiMus.Core.Entities;

public class Artist : BaseEntity
{
    public User? User { get; set; }

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; }

    [StringLength(500)] public string About { get; set; }

    public string PhotoPath { get; set; }

    public List<ArtistTrack> Tracks { get; set; } = new();
    public List<LikedArtist> Subscribers { get; set; } = new();
}