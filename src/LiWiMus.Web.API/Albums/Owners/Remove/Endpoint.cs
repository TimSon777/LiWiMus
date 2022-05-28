using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Owners.Remove;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Album> _albumRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var album = await _albumRepository.GetAlbumWithOwnersAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        var artist = album.Owners.FirstOrDefault(owner => owner.Id == request.ArtistId);

        if (artist is null)
        {
            return Results.UnprocessableEntity(
                new {detail = $"Album {album.Title} does not contains artist with Id {request.ArtistId}."});
        }

        album.Owners.Remove(artist);

        await _albumRepository.SaveChangesAsync();

        var dto = _mapper.Map<Dto>(album);
        dto.Artists = _mapper.MapList<Artist, Artists.Dto>(await _albumRepository.GetArtistsAsync(album)).ToList();
        dto.TracksCount = await _albumRepository.GetTracksCountAsync(album);
        dto.ListenersCount = await _albumRepository.GetListenersCountAsync(album);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Albums.Artists.Remove,
            async ([FromBody] Request request, IRepository<Album> repository) =>
            {
                _albumRepository = repository;
                return await HandleAsync(request);
            });
    }
}