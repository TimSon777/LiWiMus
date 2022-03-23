using System.ComponentModel.DataAnnotations;

namespace LiWiMus.Web.Areas.User.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public string Token { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}