namespace LiWiMus.Web.FileServer.Models;

public class FileDto
{
    public FileDto(string location)
    {
        Location = location;
    }

    public string Location { get; set; }
}