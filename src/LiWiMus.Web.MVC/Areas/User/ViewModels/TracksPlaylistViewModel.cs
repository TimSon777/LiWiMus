using LiWiMus.Web.MVC.ViewModels.ForListViewModels;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class TracksPlaylistViewModel
{
    public List<TrackForListViewModel> Tracks { get; set; }
    public int PlaylistId { get; set; }
}