namespace LiWiMus.Core.Entities;

public class PlaylistTrack : BaseEntity
{
    public Playlist Playlist { get; set; } = null!;
    public Track Track { get; set; } = null!;
}