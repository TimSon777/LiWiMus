namespace LiWiMus.Core.Entities;

public class ArtistTrack : BaseEntity
{
    public Artist Artist { get; set; }
    public Track Track { get; set; }
}