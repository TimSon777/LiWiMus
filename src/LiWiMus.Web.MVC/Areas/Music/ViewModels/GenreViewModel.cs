using LiWiMus.SharedKernel;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;

namespace LiWiMus.Web.MVC.Areas.Music.ViewModels;

public class GenreViewModel : HasId
{
    public string Name { get; set; }
    public List<TrackForListViewModel> Tracks { get; set; }
}