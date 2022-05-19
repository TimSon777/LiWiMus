using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Transactions;

public class Dto : BaseDto
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
}