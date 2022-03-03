namespace LiWiMus.Core.Entities;

public class User : BaseEntity
{
    public string AvatarPath { get; set; }
    public UserPlan UserPlan { get; set; }

    public int? ArtistId { get; set; }
    public Artist Artist { get; set; }

    public List<LikedAlbum> LikedAlbums { get; set; } = new();
    public List<LikedArtist> LikedArtists { get; set; } = new();
    public List<LikedPlaylist> LikedPlaylists { get; set; } = new();
    public List<LikedSong> LikedSongs { get; set; } = new();
    public List<LikedUser> Subscribers { get; set; } = new();
    public List<LikedUser> LikedUsers { get; set; } = new();
}