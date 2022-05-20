using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.API.Albums.Owners.List;

public class Request
{
    [FromRoute] public int AlbumId { get; set; }

    [FromQuery] public int Page { get; set; }

    [FromQuery] public int ItemsPerPage { get; set; }

    public Request(int albumId, int page, int itemsPerPage)
    {
        AlbumId = albumId;
        Page = page;
        ItemsPerPage = itemsPerPage;
    }
}