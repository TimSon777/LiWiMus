using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Web.Areas.User.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
}