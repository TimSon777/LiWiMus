using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Albums.Owners.Remove;

public class Request : HasId
{
    public int ArtistId { get; set; }
}