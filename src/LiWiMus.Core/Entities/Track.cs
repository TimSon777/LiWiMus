namespace LiWiMus.Core.Entities;

public class Track : BaseEntity
{
    public List<ArtistTrack> Artists { get; set; } = new();

    public Album Album { get; set; }
    public Genre Genre { get; set; }
    public string Name { get; set; }
    public DateOnly PublishedAt { get; set; }
    public string PathToFile { get; set; }

    public List<LikedSong> Subscribers { get; set; } = new();
    public List<PlaylistTrack> Playlists { get; set; }
}