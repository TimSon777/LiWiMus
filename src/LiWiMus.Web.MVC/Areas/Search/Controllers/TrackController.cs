using AutoMapper;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
public class TrackController : Controller
{
    private readonly IRepository<Track> _trackRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<Core.Users.User> _userManager;

    public TrackController(IRepository<Track> trackRepository, IMapper mapper, UserManager<Core.Users.User> userManager)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    private async Task<TrackListViewModel> GetTracks(SearchViewModel searchVm)
    {
        var user = await _userManager.GetUserAsync(User);
        
        var tracks = await _trackRepository
            .ListAsync(new TracksByTitleSpec(searchVm.Title, searchVm.Skip, searchVm.Take));

        var trackVms = _mapper.MapList<Track, TrackViewModel>(tracks);
        var playlistVms = _mapper.MapList<Playlist, PlaylistGeneralInfoViewModel>(user.Playlists);
        
        return new TrackListViewModel
        {
            Playlists = playlistVms,
            Tracks = trackVms
        };
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetTracks(SearchViewModel.Default));
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        return PartialView("TracksPartial", await GetTracks(searchVm));
    }
}