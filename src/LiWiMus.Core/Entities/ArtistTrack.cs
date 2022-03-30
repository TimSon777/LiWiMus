namespace LiWiMus.Core.Entities;

public class ArtistTrack : BaseEntity
{
    public Artist Artist { get; set; } = null!;
    public int ArtistId { get; set; }
    public Track Track { get; set; } = null!;
    public int TrackId { get; set; }
}