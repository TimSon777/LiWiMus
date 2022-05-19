using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Owners.Add;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Album> _albumRepository = null!;
    private IRepository<Artist> _artistRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        var artist = await _artistRepository.GetByIdAsync(request.ArtistId);

        if (artist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Artists, request.ArtistId);
        }

        if (album.Owners.Contains(artist))
        {
            return Results.Ok("Artist has already added");
        }
        
        album.Owners.Add(artist);
        await _albumRepository.SaveChangesAsync();

        var dto = _mapper.Map<Dto>(album);
        dto.Artists = _mapper.MapList<Artist, Artists.Dto>(await _albumRepository.GetArtistsAsync(album)).ToList();
        dto.TracksCount = await _albumRepository.GetTracksCountAsync(album);
        dto.ListenersCount = await _albumRepository.GetListenersCountAsync(album);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Albums.Owners.Add,
            async (Request request, IRepository<Album> albumRepository, IRepository<Artist> artistRepository) =>
            {
                _albumRepository = albumRepository;
                _artistRepository = artistRepository;
                return await HandleAsync(request);
            });
    }
}