using LiWiMus.SharedKernel;

namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class TrackViewModel : HaveId
{
    public string Name { get; set; }
    public string PathToFile { get; set; }
    public IEnumerable<ArtistForTrackListViewModel> Owners { get; set; }
    public AlbumForTrackViewModel Album { get; set; }
}