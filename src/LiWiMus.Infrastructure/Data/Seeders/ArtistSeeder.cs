using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Enums;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class ArtistSeeder : ISeeder
{
    private readonly IRepository<Artist> _artistRepository;
    private readonly IRepository<Track> _trackRepository;
    private readonly IRepository<Album> _albumRepository;
    private readonly UserManager<User> _userManager;

    public ArtistSeeder(IRepository<Artist> artistRepository,
        IRepository<Track> trackRepository,
        IRepository<Album> albumRepository,
        UserManager<User> userManager)
    {
        _artistRepository = artistRepository;
        _trackRepository = trackRepository;
        _albumRepository = albumRepository;
        _userManager = userManager;
    }

    public async Task SeedAsync(EnvironmentType environmentType)
    {
        const string userName = "MockUser_Artist";
        
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
                    Email = "mockEmail@mock.mock_Artist",
                    Gender = Gender.Male,
                    AvatarLocation = "Location",
                    UserName = userName
                };

                await _userManager.CreateAsync(user, "Password");
                
                var album = new Album
                {
                    Title = "MockAlbum_Artist",
                    CoverLocation = "Location"
                };

                await _albumRepository.AddAsync(album);
                
                var track = new Track
                {
                    Duration = 120,
                    Name = "MockTrack_Artist",
                    FileLocation = "Location",
                    Album = album
                };

                await _trackRepository.AddAsync(track);

                var artist = new Artist
                {
                    About = "About",
                    Name = "MockArtist_Artist",
                    PhotoLocation = "Location",
                    UserArtists = new List<UserArtist>
                    {
                        new()
                        {
                            User = user
                        }
                    },
                    Albums = new List<Album> { album },
                    Tracks = new List<Track> { track }
                };

                await _artistRepository.AddAsync(artist);
                break;
            case EnvironmentType.Production:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
        }
    }

    public int Priority => 1;
}