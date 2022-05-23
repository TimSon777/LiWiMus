using AutoMapper;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Shared;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
public class TrackController : Controller
{
    private readonly IRepository<Track> _trackRepository;
    private readonly IMapper _mapper;

    public TrackController(IRepository<Track> trackRepository, IMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    private async Task<IEnumerable<TrackForListViewModel>> GetTracks(SearchViewModel searchVm)
    {
        var tracks = await _trackRepository.PaginateWithTitleAsync(_mapper.Map<PaginationWithTitle>(searchVm));
        return _mapper.MapList<Track, TrackForListViewModel>(tracks);
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