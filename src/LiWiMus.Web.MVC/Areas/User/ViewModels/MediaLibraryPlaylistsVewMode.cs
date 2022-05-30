using LiWiMus.Web.MVC.ViewModels.GeneralInfoViewModels;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class MediaLibraryPlaylistsVewMode
{
    public List<PlaylistGeneralInfoViewModel> Playlists { get; set; }
    public List<PlaylistGeneralInfoViewModel> SubscribedPlaylists { get; set; }
}