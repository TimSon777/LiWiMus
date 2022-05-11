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
        if (!request.Ids.Any())
        {
            return Results.Ok(new { detail = "0 lines have been changed" });
        }

        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        var count = 0;
        foreach (var id in request.Ids)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            
            if (artist is null)
            {
                return Results.Extensions.NotFoundById(EntityType.Artists, id);
            }

            if (album.Owners.Contains(artist))
            {
                continue;
            }
            
            album.Owners.Add(artist);
            count++;
        }

        await _albumRepository.SaveChangesAsync();
        return Results.Ok(new { detail = $"{count} lines have been changed" });
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Albums.Owners.Add, HandleAsync);
    }
}