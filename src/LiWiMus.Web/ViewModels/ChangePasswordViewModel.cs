using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Web.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    public string NewPassword { get; set; }
    
    [Required]
    public string OldPassword { get; set; }
}