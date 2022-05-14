using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;

namespace LiWiMus.Core.PlaylistTracks;

public class PlaylistTrack : BaseEntity
{
    public virtual Playlist Playlist { get; set; } = null!;
    public virtual Track Track { get; set; } = null!;
    public int TrackId { get; set; }
    public int PlaylistId { get; set; }
}