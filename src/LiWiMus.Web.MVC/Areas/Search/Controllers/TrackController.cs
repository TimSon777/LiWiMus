using AutoMapper;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.MVC.Areas.Search.Controllers;

[Area("Search")]
public class TrackController : Controller
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebHostEnvironment _environment;
    private readonly IRepository<Track> _trackRepository;
    private readonly IMapper _mapper;

    public TrackController(IRepository<Track> trackRepository, IWebHostEnvironment environment,
                           IAuthorizationService authorizationService, IMapper mapper)
    {
        _trackRepository = trackRepository;
        _environment = environment;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    private async Task<IEnumerable<TrackListViewModel>> GetTracks(SearchViewModel searchVm)
    {
        var tracks = await _trackRepository
            .ListAsync(new TracksByTitleSpec(searchVm.Title, searchVm.Skip, searchVm.Take));

        return _mapper.MapList<Track, TrackListViewModel>(tracks);
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

    [HttpGet]
    [Route("{trackId:int}")]
    [Authorize(DefaultPermissions.Track.Read)]
    public async Task<IActionResult> Get(int trackId)
    {
        var track = await _trackRepository.GetBySpecAsync(new TrackWithArtistsByIdSpecification(trackId));

        if (track is null)
        {
            return BadRequest();
        }

        var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, track, "SameAuthorPolicy");

        if (!authorizationResult.Succeeded)
        {
            if (User.Identity is {IsAuthenticated: true})
            {
                return new ForbidResult();
            }

            return new ChallengeResult();
        }

        var pathToTrack = Path.Combine(_environment.ContentRootPath, track.PathToFile);

        if (!System.IO.File.Exists(pathToTrack))
        {
            return BadRequest();
        }

        return PhysicalFile(pathToTrack, "audio/mpeg", true);
    }
}