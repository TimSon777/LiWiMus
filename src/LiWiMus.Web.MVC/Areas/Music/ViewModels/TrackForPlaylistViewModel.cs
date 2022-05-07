using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Music.ViewModels;

public class TrackForPlaylistViewModel
{
    public ICollection<ArtistGeneralInfoViewModel> Owners { get; set; } = null!;
    public ICollection<GenreGeneralInfoViewModel> Genres { get; set; } = null!;

    public AlbumGeneralInfoViewModel Album { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string PathToFile { get; set; } = null!;
}