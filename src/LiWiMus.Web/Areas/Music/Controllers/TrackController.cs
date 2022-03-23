using LiWiMus.Core.Entities;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.Music.Controllers;

[Area("Music")]
[ApiController]
[Route("[area]/[controller]/[action]")]
public class TrackController : ControllerBase
{
    private readonly IRepository<Track> _trackRepository;
    private readonly IWebHostEnvironment _environment;

    public TrackController(IRepository<Track> trackRepository, IWebHostEnvironment environment)
    {
        _trackRepository = trackRepository;
        _environment = environment;
    }
    
    [HttpGet]
    [Route("{trackId:int}")]
    public async Task<IActionResult> Get(int trackId)
    {
        var track = await _trackRepository.GetByIdAsync(trackId);
        
        if (track is null)
        {
            return BadRequest();
        }

        var pathToTrack = Path.Combine(_environment.ContentRootPath, track.PathToFile);

        if (!System.IO.File.Exists(pathToTrack))
        {
            return BadRequest();
        }
        
        return PhysicalFile(pathToTrack, "audio/mpeg", true);
    }
}