using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Genres;
using LiWiMus.Core.Tracks;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Tracks.Create;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;
    private IRepository<Track> _trackRepository = null!;
    private IRepository<Album> _albumRepository = null!;
    private IRepository<Genre> _genreRepository = null!;
    private IRepository<Artist> _artistRepository = null!;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var track = _mapper.Map<Track>(request);

        var album = await _albumRepository.GetByIdAsync(request.AlbumId);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.AlbumId);
        }

        track.Album = album;

        track.Genres = new List<Genre>();
        foreach (var genreId in request.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);

            if (genre is null)
            {
                return Results.Extensions.NotFoundById(EntityType.Genres, genreId);
            }

            track.Genres.Add(genre);
        }

        track.Owners = new List<Artist>();
        foreach (var ownerId in request.OwnerIds)
        {
            var artist = await _artistRepository.GetByIdAsync(ownerId);

            if (artist is null)
            {
                return Results.Extensions.NotFoundById(EntityType.Artists, ownerId);
            }

            track.Owners.Add(artist);
        }

        await _trackRepository.AddAsync(track);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Tracks.Create, async (Request request, IRepository<Track> trackRepository,
                                                         IRepository<Album> albumRepository,
                                                         IRepository<Genre> genreRepository,
                                                         IRepository<Artist> artistRepository) =>
        {
            _trackRepository = trackRepository;
            _albumRepository = albumRepository;
            _genreRepository = genreRepository;
            _artistRepository = artistRepository;
            return await HandleAsync(request);
        });
    }
}