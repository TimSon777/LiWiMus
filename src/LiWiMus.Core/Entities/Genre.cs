using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;

namespace LiWiMus.Core.Entities;

public class Genre : BaseEntity
{
    [StringLength(50)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; } = null!;
}