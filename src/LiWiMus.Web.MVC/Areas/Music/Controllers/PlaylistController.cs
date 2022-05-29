using AutoMapper;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.LikedPlaylists.Specifications;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Playlists.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Music.Controllers;

[Area(AreasConstants.Music)]
public class PlaylistController : Controller
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IRepository<LikedPlaylist> _likedPlaylists;
    private readonly IMapper _mapper;
    
    public PlaylistController(IAuthorizationService authorizationService, 
        IRepository<Playlist> playlistRepository, IMapper mapper, IRepository<LikedPlaylist> likedPlaylists)
    {
        _authorizationService = authorizationService;
        _playlistRepository = playlistRepository;
        _mapper = mapper;
        _likedPlaylists = likedPlaylists;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int playlistId)
    {
        var playlist = await _playlistRepository.GetPlaylistDetailedAsync(playlistId);
        
        if (playlist is null)
        {
            return NotFound();
        }

        var isOwner = await _authorizationService.AuthorizeAsync(User, playlist, "SameAuthorPolicy")
            is { Succeeded: true };
        if (!isOwner && !playlist.IsPublic)
        {
            return Forbid();
        }

        var playlistVm = _mapper.Map<PlaylistViewModel>(playlist);
        playlistVm.IsOwner = isOwner;
        playlistVm.CountSubscribers = await _likedPlaylists.CountSubscribersAsync(playlist.Id);
        
        return View(playlistVm);
    }
}