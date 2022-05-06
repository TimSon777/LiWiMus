using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Core.Transactions;

public class Transaction : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public decimal Amount { get; set; }

    [StringLength(100)]
    public string Description { get; set; } = null!;
}