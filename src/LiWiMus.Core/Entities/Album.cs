using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;

namespace LiWiMus.Core.Entities;

public class Album : BaseEntity
{
    [StringLength(50, MinimumLength = 5)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Title { get; set; }

    public DateOnly PublishedAt { get; set; }
    public string CoverPath { get; set; }

    public List<LikedAlbum> Subscribers { get; set; } = new();
}