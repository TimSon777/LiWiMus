using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area(AreasConstants.Search)]
public class AlbumController : Controller
{
    private readonly IRepository<Album> _albumRepository;
    private readonly IMapper _mapper;

    public AlbumController(IRepository<Album> albumRepository, 
        IMapper mapper)
    {
        _albumRepository = albumRepository;
        _mapper = mapper;
    }
    
    private async Task<IEnumerable<AlbumForListViewModel>> GetAlbumsAsync(SearchViewModel searchVm)
    {
        var pagination = _mapper.Map<Pagination>(searchVm);
        var albums = await _albumRepository.PaginateWithTitleAsync(pagination);
        return _mapper.Map<List<Album>, List<AlbumForListViewModel>>(albums);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var albums = await GetAlbumsAsync(SearchViewModel.Default);
        return View(albums);
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        var albums = await GetAlbumsAsync(searchVm);
        return PartialView("AlbumsPartial", albums);
    }
}