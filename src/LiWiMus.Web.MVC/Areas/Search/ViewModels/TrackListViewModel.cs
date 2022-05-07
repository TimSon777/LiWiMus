using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class TrackListViewModel
{
    public IEnumerable<TrackViewModel> Tracks { get; set; }
    public IEnumerable<PlaylistGeneralInfoViewModel> Playlists { get; set; }
}