namespace LiWiMus.Core.Entities;

public class ArtistAlbum : BaseEntity
{
    public Artist Artist { get; set; }
    public int ArtistId { get; set; }
    public Album Album { get; set; }
    public int AlbumId { get; set; }
}