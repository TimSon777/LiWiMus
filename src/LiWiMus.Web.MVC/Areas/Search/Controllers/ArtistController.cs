using AutoMapper;
using LiWiMus.Core.Artists.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area(AreasConstants.Search)]
public class ArtistController : Controller
{
    private readonly IRepository<Core.Artists.Artist> _artistRepository;
    private readonly IMapper _mapper;

    public ArtistController(IRepository<Core.Artists.Artist> artistRepository, 
        IMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }
    
    private async Task<IEnumerable<ArtistForListViewModel>> GetArtistsAsync(SearchViewModel searchVm)
    {
        var pagination = _mapper.Map<Pagination>(searchVm);
        var artists = await _artistRepository.PaginateAsync(pagination);
        return _mapper.Map<List<Core.Artists.Artist>, List<ArtistForListViewModel>>(artists);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var artists = await GetArtistsAsync(SearchViewModel.Default);
        return View(artists);
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        var artists = await GetArtistsAsync(searchVm);
        return PartialView("ArtistsPartial", artists);
    }
}