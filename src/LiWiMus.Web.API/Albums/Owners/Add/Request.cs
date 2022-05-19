using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Albums.Owners.Add;

public class Request : HasId
{
    public int ArtistId { get; set; }
}