namespace LiWiMus.Core.Entities;

public class PlaylistTrack : BaseEntity
{
    public Playlist Playlist { get; set; }
    public Track Track { get; set; }
}