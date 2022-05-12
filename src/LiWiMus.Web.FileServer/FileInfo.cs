namespace LiWiMus.Web.FileServer;

public class FileInfo
{
    public FileInfo(string downloadName, string contentType)
    {
        ContentType = contentType;
        DownloadName = downloadName;
    }

    public int Id { get; set; }
    public string ContentType { get; set; }
    public string DownloadName { get; set; }
}