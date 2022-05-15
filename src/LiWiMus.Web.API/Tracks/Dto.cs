using LiWiMus.SharedKernel;
using AlbumDto = LiWiMus.Web.API.Albums.Dto;
using ArtistDto = LiWiMus.Web.API.Artists.Dto;

namespace LiWiMus.Web.API.Tracks;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string FileLocation { get; set; } = null!;
    public double Duration { get; set; }

    public AlbumDto Album { get; set; } = null!;
    public ICollection<ArtistDto> Artists { get; set; } = null!;
}