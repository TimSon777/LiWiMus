using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
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
    
    private async Task<IEnumerable<AlbumForListViewModel>> GetPlaylists(SearchViewModel searchVm)
    {
        var albums = await _albumRepository.PaginateWithTitleAsync(_mapper.Map<PaginationWithTitle>(searchVm));
        return _mapper.Map<List<Album>, List<AlbumForListViewModel>>(albums);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetPlaylists(SearchViewModel.Default));
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        return PartialView("AlbumsPartial", await GetPlaylists(searchVm));
    }
}