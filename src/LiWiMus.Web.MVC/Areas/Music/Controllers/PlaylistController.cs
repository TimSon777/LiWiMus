using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Music.Controllers;

[Area("Music")]
public class PlaylistController : Controller
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IMapper _mapper;
    
    public PlaylistController(IAuthorizationService authorizationService, 
        IRepository<Playlist> playlistRepository, IMapper mapper)
    {
        _authorizationService = authorizationService;
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int playlistId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        
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
        
        return View(playlistVm);
    }
}