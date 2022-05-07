using LiWiMus.SharedKernel;
using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class TrackViewModel : HaveId
{
    public string Name { get; set; }
    public string PathToFile { get; set; }
    public IEnumerable<ArtistForTrackListViewModel> Owners { get; set; }
    public AlbumGeneralInfoViewModel Album { get; set; }
}