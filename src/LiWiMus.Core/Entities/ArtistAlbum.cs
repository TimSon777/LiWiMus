namespace LiWiMus.Core.Entities;

public class ArtistAlbum : BaseEntity
{
    public Artist Artist { get; set; } = null!;
    public int ArtistId { get; set; }
    public Album Album { get; set; } = null!;
    public int AlbumId { get; set; }
}