using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Artists.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Owners.List;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Album> _albumRepository = null!;
    private IRepository<Artist> _artistRepository = null!;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper, IValidator<Request> validator)
    {
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var albumWithArtistsSpec = new AlbumWithArtistsSpec(request.AlbumId);
        var album = await _albumRepository.GetBySpecAsync(albumWithArtistsSpec);
        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.AlbumId);
        }

        var artists = await _artistRepository.ListByAlbumAsync(album, request.Page, request.ItemsPerPage);
        var count = await _artistRepository.CountByAlbumAsync(album);

        var artistsDto = _mapper.MapList<Artist, Artists.Dto>(artists);
        var result = new PaginatedData<Artists.Dto>(request.Page, request.ItemsPerPage, count, artistsDto);
        return Results.Ok(result);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Albums.Artists.List,
            async (int albumId, int page, int itemsPerPage, IRepository<Album> albumRepository,
                   IRepository<Artist> artistRepository) =>
            {
                _albumRepository = albumRepository;
                _artistRepository = artistRepository;
                return await HandleAsync(new Request(albumId, page, itemsPerPage));
            });
    }
}