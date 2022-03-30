using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Core.Entities;

public class Transaction : BaseEntity
{
    public User User { get; set; } = null!;
    public decimal Amount { get; set; }

    [StringLength(100)]
    public string Description { get; set; } = null!;
}