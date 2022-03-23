using System.ComponentModel.DataAnnotations;
using LiWiMus.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

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
    public Artist Artist { get; set; }

    public List<LikedAlbum> LikedAlbums { get; set; } = new();
    public List<LikedArtist> LikedArtists { get; set; } = new();
    public List<LikedPlaylist> LikedPlaylists { get; set; } = new();
    public List<LikedSong> LikedSongs { get; set; } = new();
    public List<LikedUser> Subscribers { get; set; } = new();
    public List<LikedUser> LikedUsers { get; set; } = new();

    public ICollection<IdentityUserClaim<int>> Claims { get; set; }
    public ICollection<IdentityUserLogin<int>> Logins { get; set; }
    public ICollection<IdentityUserToken<int>> Tokens { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}