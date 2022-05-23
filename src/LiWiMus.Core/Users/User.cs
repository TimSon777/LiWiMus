using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Constants;
using LiWiMus.Core.FollowingUsers;
using LiWiMus.Core.LikedAlbums;
using LiWiMus.Core.LikedArtists;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.LikedSongs;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Users.Enums;

namespace LiWiMus.Core.Users;

public class User : BaseUserEntity
{
    [StringLength(50)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string? SecondName { get; set; }
    
    [StringLength(50)]
    [RegularExpression(RegularExpressions.DisableTags)]
    public string? Patronymic { get; set; }
    
    public DateOnly? BirthDate { get; set; }
    public virtual Gender? Gender { get; set; }

    public decimal Balance { get; set; }

    public string? AvatarLocation { get; set; }

    public virtual ICollection<UserArtist> UserArtists { get; set; } = null!;
    public virtual ICollection<Artist> Artists { get; set; } = null!;

    public virtual ICollection<LikedAlbum> LikedAlbums { get; set; } = null!;
    public virtual ICollection<LikedArtist> LikedArtists { get; set; } = null!;
    public virtual ICollection<LikedPlaylist> LikedPlaylists { get; set; } = null!;
    public virtual ICollection<LikedSong> LikedSongs { get; set; } = null!;
    public virtual ICollection<FollowingUser> Followers { get; set; } = null!;
    public virtual ICollection<FollowingUser> Following { get; set; } = null!;
    public virtual ICollection<Chat> UserChats { get; set; } = null!;
    public virtual ICollection<Playlist> Playlists { get; set; } = null!;
    public virtual ICollection<Role> Roles { get; set; } = null!;

    public virtual UserPlan? UserPlan { get; set; } = null!;
    public int? UserPlanId { get; set; }
}