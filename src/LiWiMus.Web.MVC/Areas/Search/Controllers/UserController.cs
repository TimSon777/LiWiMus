using AutoMapper;
using LiWiMus.Core.Shared;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels.ForListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area(AreasConstants.Search)]
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
        var pagination = _mapper.Map<Pagination>(searchVm);
        var users = await _userRepository.PaginateWithTitleAsync(pagination);
        return _mapper.Map<List<Core.Users.User>, List<UserForListViewModel>>(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await GetUsersAsync(SearchViewModel.Default);
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> ShowMore(SearchViewModel searchVm)
    {
        var users = await GetUsersAsync(searchVm);
        return PartialView("UsersPartial", users);
    }
}