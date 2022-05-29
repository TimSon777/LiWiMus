using AutoMapper;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Music.Controllers;

[Area(AreasConstants.Music)]
public class TrackController : Controller
{
    private readonly IRepository<Track> _trackRepository;
    private readonly IMapper _mapper;

    public TrackController(IRepository<Track> trackRepository, IMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int trackId)
    {
        var track = await _trackRepository.GetDetailedAsync(trackId);

        if (track is null)
        {
            return NotFound();
        }

        var trackVm = _mapper.Map<TrackViewModel>(track);
        
        return View(trackVm);
    }
}