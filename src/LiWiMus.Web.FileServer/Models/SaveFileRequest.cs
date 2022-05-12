namespace LiWiMus.Web.FileServer.Models;

public class SaveFileRequest
{
    public IFormFile File { get; set; } = null!;
}