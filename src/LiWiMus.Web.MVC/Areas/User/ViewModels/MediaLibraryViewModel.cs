using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using LiWiMus.Web.MVC.ViewModels.GeneralInfoViewModels;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class MediaLibraryViewModel
{
    public List<TrackViewModel> Tracks { get; set; }
    public List<ArtistGeneralInfoViewModel> Artists { get; set; }
    public List<PlaylistGeneralInfoViewModel> Playlists { get; set; }
    public List<PlaylistGeneralInfoViewModel> SubscribedPlaylists { get; set; }
    public List<UserGeneralInfoViewModel> Users { get; set; }
}