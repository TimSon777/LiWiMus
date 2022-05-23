using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Enums;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class AlbumSeeder : ISeeder
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Track> _trackRepository;
    private readonly IRepository<Artist> _artistRepository;

    public AlbumSeeder(UserManager<User> userManager, IRepository<Track> trackRepository, IRepository<Artist> artistRepository)
    {
        _userManager = userManager;
        _trackRepository = trackRepository;
        _artistRepository = artistRepository;
    }

    public async Task SeedAsync(EnvironmentType environmentType)
    {
        const string userName = "MockUser_Album";
                
        if (await _userManager.FindByNameAsync(userName) is not null)
        {
            return;
        }
        
        switch (environmentType)
        {
            case EnvironmentType.Development:
            case EnvironmentType.Testing:
                var user = new User
                {
                    Email = "mockEmail@mock.mock_Album",
                    AvatarLocation = "Location",
                    Gender = Gender.Male,
                    Id = 90000,
                    UserName = userName
                };

                var result = await _userManager.CreateAsync(user);
                
                if (!result.Succeeded)
                {
                    throw new SystemException();
                }

                var artist1 = new Artist
                {
                    About = "About",
                    Name = "MockArtist1_Album",
                    Id = 90000,
                    PhotoLocation = "Location"
                };

                var artist2 = new Artist
                {
                    About = "About",
                    Name = "MockArtist2_Album",
                    Id = 90001,
                    PhotoLocation = "Location"
                };

                var album = new Album
                {
                    Id = 90000,
                    CoverLocation = "Location",
                    Owners = new List<Artist> {artist1, artist2},
                    Title = "MockAlbum_Album"
                };

                var track = new Track
                {
                    Id = 90000,
                    Album = album,
                    Duration = 90,
                    Name = "MockTrack_Album",
                    Owners = new List<Artist> {artist1},
                    FileLocation = "Location"
                };

                await _trackRepository.AddAsync(track);

                var artistWithoutAlbum = new Artist
                {
                    Id = 90002,
                    About = "About",
                    Name = "MockArtist3_Album",
                    PhotoLocation = "Location"
                };

                await _artistRepository.AddAsync(artistWithoutAlbum);
                break;
            case EnvironmentType.Production:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
        }
    }

    public int Priority => 40;
}