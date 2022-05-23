using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Playlists.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
public class PlaylistController : Controller
{
    private readonly IRepository<Playlist> _playlistRepository;
    private readonly IMapper _mapper;

    public PlaylistController(IRepository<Playlist> playlistRepository, 
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }
    
    private async Task<IEnumerable<PlaylistForListViewModel>> GetPlaylists(SearchViewModel searchVm)
    {
        var playlists = await _playlistRepository.PaginateWithTitleAsync(_mapper.Map<PaginationWithTitle>(searchVm));
        var playlistVms = _mapper.Map<List<Playlist>, List<PlaylistForListViewModel>>(playlists);
        
        foreach (var vm in playlistVms)
        {
            vm.IsOwner = User.Identity?.Name == vm.Owner.UserName;
        }

        return playlistVms;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetPlaylists(SearchViewModel.Default));
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        return PartialView("PlaylistsPartial", await GetPlaylists(searchVm));
    }
}