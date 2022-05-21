using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class AlbumForListViewModel
{
    public string Title { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string CoverLocation { get; set; } = null!;
    public virtual ICollection<ArtistGeneralInfoViewModel> Owners { get; set; }
}