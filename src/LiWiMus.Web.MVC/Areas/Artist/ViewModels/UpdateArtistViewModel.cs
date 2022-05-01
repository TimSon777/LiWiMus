using LiWiMus.Core.Models;
using LiWiMus.Web.Shared.Binders.ImageBinder;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Artist.ViewModels;

public class UpdateArtistViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string About { get; set; } = null!;

    [ModelBinder(typeof(ImageModelBinder))]
    public ImageInfo? Photo { get; set; }
}