using LiWiMus.SharedKernel;

namespace LiWiMus.Web.MVC.ViewModels;

public class AlbumGeneralInfoViewModel : HaveId
{
    public string Title { get; set; }
    public string CoverPath { get; set; }
}