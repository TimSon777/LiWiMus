using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;

namespace LiWiMus.Core.Entities;

public class Plan : BaseEntity
{
    [StringLength(50)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Name { get; set; }

    [StringLength(500)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string Description { get; set; }

    public decimal PricePerMonth { get; set; }
}