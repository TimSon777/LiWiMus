using LiWiMus.SharedKernel;
using LiWiMus.Web.MVC.ViewModels.GeneralInfoViewModels;

namespace LiWiMus.Web.MVC.ViewModels.ForListViewModels;

public class TrackForListViewModel : HasId
{
    public string Name { get; set; }
    public string PathToFile { get; set; }
    public IEnumerable<ArtistGeneralInfoViewModel> Owners { get; set; }
    public AlbumGeneralInfoViewModel Album { get; set; }
}