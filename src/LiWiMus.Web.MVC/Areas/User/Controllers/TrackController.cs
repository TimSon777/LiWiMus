using FormHelper;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.User.Controllers;

[Area("User")]
public class TrackController : Controller
{
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IRepository<Track> _trackRepository;
    private readonly IRepository<PlaylistTrack> _playlistTrackRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebHostEnvironment _environment;

    public TrackController(IRepository<Playlist> playlistRepository, 
        IAuthorizationService authorizationService, 
        IRepository<Track> trackRepository, IRepository<PlaylistTrack> playlistTrackRepository, IWebHostEnvironment environment)
    {
        _playlistRepository = playlistRepository;
        _authorizationService = authorizationService;
        _trackRepository = trackRepository;
        _playlistTrackRepository = playlistTrackRepository;
        _environment = environment;
    }

    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> AddToPlaylist(int playlistId, int trackId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);

        if (playlist is null)
        {
            return NotFound();
        }
        
        if (await _authorizationService.AuthorizeAsync(User, playlist, "SameAuthorPolicy")
            is {Succeeded: false})
        {
            return Forbid();
        }

        var track = await _trackRepository.GetByIdAsync(trackId);
        
        if (track is null)
        {
            return NotFound();
        }

        var playlistTrack = new PlaylistTrack
        {
            Playlist = playlist,
            Track = track
        };

        await _playlistTrackRepository.AddAsync(playlistTrack);

        return FormResult.CreateSuccessResult("Ok");
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Download(int trackId)
    {
        var track = await _trackRepository.GetBySpecAsync(new TrackWithArtistsByIdSpecification(trackId));

        if (track is null)
        {
            return BadRequest();
        }

        // var authorizationResult = await _authorizationService
        //     .AuthorizeAsync(User, track, "SameAuthorPolicy");
        //
        // if (!authorizationResult.Succeeded)
        // {
        //     if (User.Identity is {IsAuthenticated: true})
        //     {
        //         return new ForbidResult();
        //     }
        //
        //     return new ChallengeResult();
        // }

        var pathToTrack = Path.Combine(_environment.ContentRootPath, track.PathToFile);

        if (!System.IO.File.Exists(pathToTrack))
        {
            return BadRequest();
        }

        return PhysicalFile(pathToTrack, "audio/mpeg");
    }
}