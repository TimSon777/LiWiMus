using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Constants;
using LiWiMus.Core.LikedAlbums;
using LiWiMus.Core.LikedArtists;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.LikedSongs;
using LiWiMus.Core.LikedUsers;
using LiWiMus.Core.UserClaims;
using LiWiMus.Core.UserRoles;
using LiWiMus.Core.Users.Enums;
using Microsoft.AspNetCore.Identity;

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
    public Gender? Gender { get; set; }

    public decimal Balance { get; set; }

    public string? AvatarPath { get; set; }

    public int? ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;
    public List<LikedAlbum> LikedAlbums { get; set; } = new();
    public List<LikedArtist> LikedArtists { get; set; } = new();
    public List<LikedPlaylist> LikedPlaylists { get; set; } = new();
    public List<LikedSong> LikedSongs { get; set; } = new();
    public List<LikedUser> Subscribers { get; set; } = new();
    public List<LikedUser> LikedUsers { get; set; } = new();
    public List<UserClaim> Claims { get; set; } = new();
    public List<IdentityUserLogin<int>> Logins { get; set; } = new();
    public List<IdentityUserToken<int>> Tokens { get; set; } = new();
    public List<UserRole> UserRoles { get; set; } = new();
    public List<Chat> UserChats { get; set; } = new();
}