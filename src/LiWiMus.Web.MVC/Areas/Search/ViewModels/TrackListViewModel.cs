namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class TrackListViewModel
{
    public string Name { get; set; }
    public string PathToFile { get; set; }
    public IEnumerable<ArtistForTrackListViewModel> Owners { get; set; }
    public AlbumForTrackViewModel Album { get; set; }
}