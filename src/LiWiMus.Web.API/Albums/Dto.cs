using LiWiMus.SharedKernel;
using ArtistDto = LiWiMus.Web.API.Artists.Dto;

namespace LiWiMus.Web.API.Albums;

public class Dto : BaseDto
{
    public string Title { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string CoverLocation { get; set; } = null!;
    public ICollection<ArtistDto> Artists { get; set; } = new List<ArtistDto>();

    public int TracksCount { get; set; }
    public int ListenersCount { get; set; }
}