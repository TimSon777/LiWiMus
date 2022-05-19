namespace LiWiMus.Web.API.Transactions.Create;

public class Request
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
}