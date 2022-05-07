using FormHelper;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Tracks;
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

    public TrackController(IRepository<Playlist> playlistRepository, 
        IAuthorizationService authorizationService, 
        IRepository<Track> trackRepository, IRepository<PlaylistTrack> playlistTrackRepository)
    {
        _playlistRepository = playlistRepository;
        _authorizationService = authorizationService;
        _trackRepository = trackRepository;
        _playlistTrackRepository = playlistTrackRepository;
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
}