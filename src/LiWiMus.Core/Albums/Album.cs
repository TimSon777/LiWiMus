using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Constants;
using LiWiMus.Core.LikedAlbums;
using LiWiMus.Core.Shared.Interfaces;
using LiWiMus.Core.Tracks;

namespace LiWiMus.Core.Albums;

public class Album : BaseEntity, IResource.WithMultipleOwners<Artist>
{
    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Title { get; set; } = null!;

    public DateOnly PublishedAt { get; set; }
    public string CoverLocation { get; set; } = null!;

    public virtual ICollection<LikedAlbum> Subscribers { get; set; } = null!;
    public virtual ICollection<Artist> Owners { get; set; } = null!;
    public virtual ICollection<Track> Tracks { get; set; } = null!;
}