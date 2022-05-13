using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using MinimalApi.Endpoint;
using LiWiMus.Web.API.Shared.Add;
using LiWiMus.Web.API.Shared.Extensions;

namespace LiWiMus.Web.API.Albums.Owners.Add;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IRepository<Album> _albumRepository;
    private readonly IRepository<Artist> _artistRepository;

    public Endpoint(IRepository<Album> albumRepository, IRepository<Artist> artistRepository)
    {
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        var artist = await _artistRepository.GetByIdAsync(request.AddedId);

        if (artist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Artists, request.AddedId);
        }

        if (album.Owners.Contains(artist))
        {
            return Results.Ok("Artist has already added");
        }
        
        album.Owners.Add(artist);
        await _albumRepository.SaveChangesAsync();
        return Results.Ok();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Albums.Owners.Add, HandleAsync);
    }
}