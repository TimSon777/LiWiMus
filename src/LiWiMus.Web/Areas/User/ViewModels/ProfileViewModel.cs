using System.ComponentModel.DataAnnotations;
using LiWiMus.Web.Binders.ImageBinder;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace LiWiMus.Web.Areas.User.ViewModels;

public class ProfileViewModel
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? Patronymic { get; set; }
    
    public bool IsMale { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    public bool IsAccountOwner { get; set; }
    
    [ModelBinder(typeof(ImageModelBinder))]
    public Image? Avatar { get; set; }
}