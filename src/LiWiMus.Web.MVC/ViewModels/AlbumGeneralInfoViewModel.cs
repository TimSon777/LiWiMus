using LiWiMus.SharedKernel;

namespace LiWiMus.Web.MVC.ViewModels;

public class AlbumGeneralInfoViewModel : HasId
{
    public string Title { get; set; }
    public string CoverPath { get; set; }
}