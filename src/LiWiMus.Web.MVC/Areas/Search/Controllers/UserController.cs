using AutoMapper;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Shared;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
public class UserController : Controller
{
    private readonly IRepository<Core.Users.User> _userRepository;
    private readonly IMapper _mapper;

    public UserController(IRepository<Core.Users.User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    private async Task<IEnumerable<UserForListViewModel>> GetUsersAsync(SearchViewModel searchVm)
    {
        var users = await _userRepository.PaginateWithTitleAsync(_mapper.Map<PaginationWithTitle>(searchVm));
        return _mapper.Map<List<Core.Users.User>, List<UserForListViewModel>>(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetUsersAsync(SearchViewModel.Default));
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        return PartialView("UsersPartial", await GetUsersAsync(searchVm));
    }
}