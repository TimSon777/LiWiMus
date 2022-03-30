namespace LiWiMus.Web.Areas.User.ViewModels;

public class PaymentViewModel
{
    public int Amount { get; set; }
    public string ReturnUrl { get; set; } = null!;
    public string? Reason { get; set; }
}