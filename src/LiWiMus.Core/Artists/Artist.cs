using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Constants;
using LiWiMus.Core.LikedArtists;
using LiWiMus.Core.Shared.Interfaces;
using LiWiMus.Core.Tracks;

namespace LiWiMus.Core.Artists;

public class Artist : BaseEntity, IResource.WithMultipleOwners<User>
{
    public virtual ICollection<UserArtist> UserArtists { get; set; } = null!;
    public virtual ICollection<User> Owners { get; set; } = null!;

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string About { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = null!;
    public virtual ICollection<LikedArtist> Subscribers { get; set; } = null!;
    public virtual ICollection<Album> Albums { get; set; } = null!;
}