using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Transactions;

public class Dto : BaseDto
{
    public Users.Dto User { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
}