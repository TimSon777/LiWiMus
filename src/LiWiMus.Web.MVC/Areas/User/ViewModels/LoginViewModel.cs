using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class LoginViewModel
{
    [Required] public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}