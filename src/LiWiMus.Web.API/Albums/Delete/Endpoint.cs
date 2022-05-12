using LiWiMus.Core.Albums;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Delete;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IRepository<Album> _albumRepository;

    public Endpoint(IRepository<Album> albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var album = await _albumRepository.GetByIdAsync(id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, id);
        }
        
        await _albumRepository.DeleteAsync(album);

        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Albums.Delete, HandleAsync);
    }
}