namespace LiWiMus.Core.Entities;

public class Artist : BaseEntity
{
    public User User { get; set; }
    public string Name { get; set; }
    public string About { get; set; }
    public string PhotoPath { get; set; }

    public List<ArtistTrack> Tracks { get; set; } = new();
    public List<LikedArtist> Subscribers { get; set; } = new();
}