using LiWiMus.Core.Permissions;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Music.Controllers;

[Area("Music")]
[ApiController]
[Route("[area]/[controller]/[action]")]
public class TrackController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IWebHostEnvironment _environment;
    private readonly IRepository<Track> _trackRepository;

    public TrackController(IRepository<Track> trackRepository, IWebHostEnvironment environment,
                           IAuthorizationService authorizationService)
    {
        _trackRepository = trackRepository;
        _environment = environment;
        _authorizationService = authorizationService;
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