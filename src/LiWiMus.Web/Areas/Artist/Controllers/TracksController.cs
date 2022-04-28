using AutoMapper;
using FormHelper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists.Specifications;
using LiWiMus.Core.Genres;
using LiWiMus.Core.Genres.Specifications;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.Infrastructure.Extensions;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Areas.Artist.ViewModels;
using LiWiMus.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LiWiMus.Web.Areas.Artist.Controllers;

[Area("Artist")]
[Route("Artist/{artistId:int}/[controller]")]
public class TracksController : Controller
{
    private readonly IRepository<Core.Artists.Artist> _artistRepository;
    private readonly IRepository<Core.Tracks.Track> _tracksRepository;
    private readonly IRepository<Core.Albums.Album> _albumsRepository;
    private readonly IRepository<Core.Genres.Genre> _genresRepository;
    private readonly IOptions<DataSettings> _dataSettings;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;

    public TracksController(IRepository<Core.Artists.Artist> artistRepository, IRepository<Track> tracksRepository,
                            IOptions<DataSettings> dataSettings, IAuthorizationService authorizationService,
                            IMapper mapper, IRepository<Album> albumsRepository, IRepository<Genre> genresRepository)
    {
        _artistRepository = artistRepository;
        _tracksRepository = tracksRepository;
        _dataSettings = dataSettings;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _albumsRepository = albumsRepository;
        _genresRepository = genresRepository;
    }

    [HttpGet("")]
    // GET
    public async Task<IActionResult> Index(int artistId)
    {
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithTracksAndOwnersByIdSpec(artistId));

        if (artist is null)
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }
        
        return View(artist.Tracks);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int artistId, int id)
    {
        var track = await _tracksRepository.GetBySpecAsync(new DetailedTrackByIdSpec(id));
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(artistId));

        if (track is null || artist is null || track.Owners.All(a => a.Id != artistId))
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false} ||
            await _authorizationService.AuthorizeAsync(User, track, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }
            
        return View(track);
    }

        
    [HttpPut("{id:int}")]
    [FormValidator]
    public async Task<IActionResult> Update(int artistId, UpdateTrackViewModel viewModel)
    {
        var track = await _tracksRepository.GetBySpecAsync(new DetailedTrackByIdSpec(viewModel.Id));
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(artistId));

        if (track is null || artist is null || track.Owners.All(a => a.Id != artistId))
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false} ||
            await _authorizationService.AuthorizeAsync(User, track, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }
        
        if (viewModel.ArtistsIds is not null)
        {
            if (viewModel.ArtistsIds.All(a => a != artistId))
            {
                return BadRequest();
            }
            var artists = await _artistRepository.ListAsync(new ArtistsByIdsSpec(viewModel.ArtistsIds));
            track.Owners = artists;
        }

        if (viewModel.GenresIds is not null)
        {
            var genres = await _genresRepository.ListAsync(new GenresByIdsSpec(viewModel.GenresIds));
            track.Genres = genres;
        }

        _mapper.Map(viewModel, track);
        if (viewModel.File is not null)
        {
            track.PathToFile = await viewModel.File.SaveWithRandomNameAsync(_dataSettings.Value.TracksDirectory);
        }

        await _tracksRepository.UpdateAsync(track);
        return FormResult.CreateSuccessResult("Updated successfully");
    }

    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("[action]")]
    [FormValidator]
    public async Task<IActionResult> Create(int artistId, CreateTrackViewModel viewModel)
    {
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(artistId));
        var album = await _albumsRepository.GetBySpecAsync(new DetailedAlbumByIdSpec(viewModel.AlbumId));

        if (artist is null || album is null || album.Owners.All(a => a.Id != artistId))
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false} ||
            await _authorizationService.AuthorizeAsync(User, album, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }
        
        var filePath = await viewModel.File.SaveWithRandomNameAsync(_dataSettings.Value.CoversDirectory);
        
        var artists = new List<Core.Artists.Artist> {artist};

        var track = new Track
        {
            Name = viewModel.Name,
            PublishedAt = viewModel.PublishedAt,
            Owners = artists,
            PathToFile = filePath,
            Album = album
        };
        track = await _tracksRepository.AddAsync(track);

        return FormResult.CreateSuccessResult("Created successfully",
            Url.Action("Details", "Tracks", new {Area = "Artist", track.Id}));
    }

    
    [HttpDelete("{id:int}")]
    [FormValidator]
    public async Task<IActionResult> Delete(int artistId, int id)
    {
        var track = await _tracksRepository.GetBySpecAsync(new DetailedTrackByIdSpec(id));
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(artistId));

        if (track is null || artist is null || track.Owners.All(a => a.Id != artistId))
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false} ||
            await _authorizationService.AuthorizeAsync(User, track, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }

        await _tracksRepository.DeleteAsync(track);
        return FormResult.CreateSuccessResult("Removed successfully");
    }
}