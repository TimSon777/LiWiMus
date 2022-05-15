using LiWiMus.Core.Playlists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.Delete;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Playlist> _repository = null!;

    public async Task<IResult> HandleAsync(int id)
    {
        var playlist = await _repository.GetByIdAsync(id);

        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, id);
        }

        await _repository.DeleteAsync(playlist);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Playlists.Delete, async (int id, IRepository<Playlist> repository) =>
        {
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}