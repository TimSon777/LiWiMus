using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities.Interfaces;

namespace LiWiMus.Core.Entities;

public class Album : BaseEntity, IMultipleArtistOwnersResource
{

    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Title { get; set; } = null!;

    public DateOnly PublishedAt { get; set; }
    public string CoverPath { get; set; } = null!;

    public List<LikedAlbum> Subscribers { get; set; } = new();
    public List<Artist> Artists { get; set; } = new();
    public List<ArtistAlbum> ArtistAlbums { get; set; } = new();
}