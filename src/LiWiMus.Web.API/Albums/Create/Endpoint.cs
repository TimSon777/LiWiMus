using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Create;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;
    private IRepository<Album> _albumRepository = null!;
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

        var album = _mapper.Map<Album>(request);
        album.Owners = new List<Artist>();

        foreach (var artistId in request.ArtistIds)
        {
            var artist = await _artistRepository.GetByIdAsync(artistId);

            if (artist is null)
            {
                return Results.Extensions.NotFoundById(EntityType.Artists, artistId);
            }

            album.Owners.Add(artist);
        }

        await _albumRepository.AddAsync(album);

        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Albums.Create,
            async (Request request, IRepository<Album> albumRepository, IRepository<Artist> artistRepository) =>
            {
                _albumRepository = albumRepository;
                _artistRepository = artistRepository;
                return await HandleAsync(request);
            });
    }
}