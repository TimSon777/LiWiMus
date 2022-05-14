using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.API.Playlists.Tracks.List;

public class Request
{
    [FromRoute] public int PlaylistId { get; set; }

    [FromQuery] public int Page { get; set; }

    [FromQuery] public int ItemsPerPage { get; set; }

    public Request(int playlistId, int page, int itemsPerPage)
    {
        PlaylistId = playlistId;
        Page = page;
        ItemsPerPage = itemsPerPage;
    }
}