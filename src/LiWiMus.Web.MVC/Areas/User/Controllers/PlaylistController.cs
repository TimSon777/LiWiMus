using AutoMapper;
using FormHelper;
using LiWiMus.Core.LikedPlaylists;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.User.ViewModels;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.User.Controllers;

[Area("User")]
public class PlaylistController : Controller
{
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IRepository<LikedPlaylist> _likedPlaylistRepository;
    private readonly IFormFileSaver _formFileSaver;
    private readonly UserManager<Core.Users.User> _userManager;

    public PlaylistController(IMapper mapper, IRepository<Playlist> playlistRepository, IFormFileSaver formFileSaver, UserManager<Core.Users.User> userManager, IRepository<LikedPlaylist> likedPlaylistRepository)
    {
        _mapper = mapper;
        _playlistRepository = playlistRepository;
        _formFileSaver = formFileSaver;
        _userManager = userManager;
        _likedPlaylistRepository = likedPlaylistRepository;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost, FormValidator]
    public async Task<IActionResult> Create(CreatePlaylistViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return FormResult.CreateErrorResultWithObject(vm);
        }
        
        var user = await _userManager.GetUserAsync(User);

        var mappedPlaylist = _mapper.Map<Playlist>(vm);
        var path = await _formFileSaver.SaveWithRandomNameAsync(vm.Picture, DataType.Picture);
        mappedPlaylist.PhotoLocation = path;
        mappedPlaylist.Owner = user;
        var playlist = await _playlistRepository.AddAsync(mappedPlaylist);
        return FormResult.CreateSuccessResult("Ok", $"/Music/Playlist/Index?playlistId={playlist.Id}");
    }

    [HttpPost, FormValidator]
    public async Task<IActionResult> Like(int playlistId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        
        if (playlist is null)
        {
            return FormResult.CreateErrorResult("Playlist is not found.");
        }

        var user = await _userManager.GetUserAsync(User);

        if (user.Playlists.Any(p => p == playlist))
        {
            return FormResult.CreateErrorResult("It is your playlist.");
        }

        if (user.LikedPlaylists.Any(lp => lp.Playlist == playlist))
        {
            return FormResult.CreateErrorResult("You has already subscribed to this playlist.");
        }

        var lp = new LikedPlaylist
        {
            Playlist = playlist,
            User = user
        };

        await _likedPlaylistRepository.AddAsync(lp);
        return FormResult.CreateSuccessResult("Ok");
    }
}