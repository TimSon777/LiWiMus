namespace LiWiMus.Web.Areas.User.ViewModels;

public class ExternalLoginConfirmationViewModel
{
    public string? ReturnUrl { get; set; }
    public string? ProviderDisplayName { get; set; }

    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
}