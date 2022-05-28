using AutoMapper;
using LiWiMus.Core.Genres;
using LiWiMus.Core.Genres.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.MVC.Areas.Music.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.Music.Controllers;

[Area(AreasConstants.Music)]
public class GenreController : Controller
{
    private readonly IRepository<Genre> _genreRepository;
    private readonly IMapper _mapper;

    public GenreController(IRepository<Genre> genreRepository, IMapper mapper)
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int genreId)
    {
        var genre = await _genreRepository.GenreWithPopularSongsAsync(genreId);

        if (genre is null)
        {
            return NotFound();
        }

        var genreVm = _mapper.Map<GenreViewModel>(genre);
        
        return View(genreVm);
    }
}