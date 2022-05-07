using AutoMapper;
using FormHelper;
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
    private readonly IFormFileSaver _formFileSaver;
    private readonly UserManager<Core.Users.User> _userManager;

    public PlaylistController(IMapper mapper, IRepository<Playlist> playlistRepository, IFormFileSaver formFileSaver, UserManager<Core.Users.User> userManager)
    {
        _mapper = mapper;
        _playlistRepository = playlistRepository;
        _formFileSaver = formFileSaver;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> Create(CreatePlaylistViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return FormResult.CreateErrorResultWithObject(vm);
        }
        
        var user = await _userManager.GetUserAsync(User);

        var mappedPlaylist = _mapper.Map<Playlist>(vm);
        var path = await _formFileSaver.SaveWithRandomNameAsync(vm.Picture, DataType.Picture);
        mappedPlaylist.PhotoPath = path;
        mappedPlaylist.Owner = user;
        var playlist = await _playlistRepository.AddAsync(mappedPlaylist);
        return FormResult.CreateSuccessResult("Ok", $"/Music/Playlist/Index?playlistId={playlist.Id}");
    }
}