namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class CreatePlaylistViewModel
{
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public IFormFile Picture { get; set; }
}