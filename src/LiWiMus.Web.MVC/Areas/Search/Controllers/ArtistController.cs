using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
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
    
    private async Task<IEnumerable<ArtistForListViewModel>> GetArtists(SearchViewModel searchVm)
    {
        var artists = await _artistRepository.PaginateAsync(_mapper.Map<PaginationWithTitle>(searchVm));
        return _mapper.Map<List<Core.Artists.Artist>, List<ArtistForListViewModel>>(artists);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetArtists(SearchViewModel.Default));
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        return PartialView("ArtistsPartial", await GetArtists(searchVm));
    }
}