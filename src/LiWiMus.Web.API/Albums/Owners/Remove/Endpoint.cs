using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;
using LiWiMus.Web.API.Shared.Remove;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.API.Albums.Owners.Remove;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IRepository<Album> _albumRepository;

    public Endpoint(IRepository<Album> albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<IResult> HandleAsync([FromBody] Request request)
    {
        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        var artist = album.Owners.FirstOrDefault(owner => owner.Id == request.DeletedId);

        if (artist is null)
        {
            return Results.UnprocessableEntity(
                new { detail = $"Album {album.Title} does not contains artist with Id {request.DeletedId}." });
        }

        album.Owners.Remove(artist);

        await _albumRepository.SaveChangesAsync();
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Albums.Owners.Remove, HandleAsync);
    }
}