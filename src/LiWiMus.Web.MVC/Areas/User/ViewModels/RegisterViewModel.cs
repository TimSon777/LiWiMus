using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class RegisterViewModel
{
    [Required] public string UserName { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string PasswordConfirm { get; set; }
}