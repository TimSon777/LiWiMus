using AutoMapper;
using LiWiMus.Core.Shared;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area(AreasConstants.Search)]
public class TrackController : Controller
{
    private readonly IRepository<Track> _trackRepository;
    private readonly IMapper _mapper;

    public TrackController(IRepository<Track> trackRepository, IMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    private async Task<IEnumerable<TrackForListViewModel>> GetTracksAsync(SearchViewModel searchVm)
    {
        var _ = Request;
        var pagination = _mapper.Map<Pagination>(searchVm);
        var tracks = await _trackRepository.PaginateWithTitleAsync(pagination);
        return _mapper.MapList<Track, TrackForListViewModel>(tracks);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tracks = await GetTracksAsync(SearchViewModel.Default);
        return View(tracks);
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        var tracks = await GetTracksAsync(searchVm);
        return PartialView("TracksPartial", tracks);
    }
}