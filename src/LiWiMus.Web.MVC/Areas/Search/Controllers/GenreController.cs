using AutoMapper;
using LiWiMus.Core.Genres;
using LiWiMus.Core.Genres.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area(AreasConstants.Search)]
public class GenreController : Controller
{
    private readonly IRepository<Genre> _artistRepository;
    private readonly IMapper _mapper;

    public GenreController(IRepository<Genre> artistRepository, 
        IMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }
    
    private async Task<IEnumerable<GenreForListViewModel>> GetGenresAsync(SearchViewModel searchVm)
    {
        var pagination = _mapper.Map<Pagination>(searchVm);
        var genres = await _artistRepository.PaginateAsync(pagination);
        return _mapper.Map<List<Genre>, List<GenreForListViewModel>>(genres);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var genres = await GetGenresAsync(SearchViewModel.Default);
        return View(genres);
    }
    
    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        var genres = await GetGenresAsync(searchVm);
        return PartialView("GenresPartial", genres);
    }
}