#region

using System.Security.Claims;
using AutoMapper;
using FormHelper;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Artists.Specifications;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Artist.ViewModels;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.Controllers;

[Area("Artist")]
[Route("[area]")]
public class HomeController : Controller
{
    private readonly IFormFileSaver _formFileSaver;
    private readonly IRepository<Core.Artists.Artist> _artistRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;
    private readonly UserManager<Core.Users.User> _userManager;

    public HomeController(UserManager<Core.Users.User> userManager,
                          IAuthorizationService authorizationService,
                          IMapper mapper, IRepository<Core.Artists.Artist> artistRepository,
                          IFormFileSaver formFileSaver)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _artistRepository = artistRepository;
        _formFileSaver = formFileSaver;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString is null)
        {
            return Challenge();
        }

        var userId = int.Parse(userIdString);

        var artists = await _artistRepository.ListAsync(new ArtistsByUserIdSpec(userId));

        return View(artists);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Profile(int id)
    {
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(id));

        if (artist is null)
        {
            return NotFound();
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false})
        {
            return Forbid();
        }

        return View(artist);
    }

    [HttpPut("")]
    [FormValidator]
    public async Task<IActionResult> Update(UpdateArtistViewModel viewModel)
    {
        var artist = await _artistRepository.GetBySpecAsync(new ArtistWithOwnersByIdSpec(viewModel.Id));

        if (artist is null)
        {
            return FormResult.CreateErrorResult("No such artist");
        }

        if (await _authorizationService.AuthorizeAsync(User, artist, "SameAuthorPolicy") is {Succeeded: false})
        {
            return FormResult.CreateErrorResult("Access denied");
        }

        _mapper.Map(viewModel, artist);
        if (viewModel.Photo is not null)
        {
            FileHelper.DeleteIfExists(artist.PhotoPath);
            artist.PhotoPath = await _formFileSaver.SaveWithRandomNameAsync(viewModel.Photo);
        }

        await _artistRepository.UpdateAsync(artist);
        return FormResult.CreateSuccessResult("Updated successfully");
    }

    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("[action]")]
    [FormValidator]
    public async Task<IActionResult> Create(CreateArtistViewModel viewModel)
    {
        var user = await _userManager.GetUserAsync(User);

        var photoPath = await _formFileSaver.SaveWithRandomNameAsync(viewModel.Photo);

        var artist = new Core.Artists.Artist
        {
            Name = viewModel.Name,
            About = viewModel.About,
            PhotoPath = photoPath
        };
        artist.UserArtists.Add(new UserArtist {User = user, Artist = artist});
        artist = await _artistRepository.AddAsync(artist);

        return FormResult.CreateSuccessResult("Created successfully",
            Url.Action("Profile", "Home", new {Area = "Artist", artist.Id}));
    }
}