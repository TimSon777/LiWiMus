using LiWiMus.SharedKernel;
using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class TrackForListViewModel : HasId
{
    public string Name { get; set; }
    public string PathToFile { get; set; }
    public IEnumerable<ArtistGeneralInfoViewModel> Owners { get; set; }
    public AlbumGeneralInfoViewModel Album { get; set; }
}