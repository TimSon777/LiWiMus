using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Playlists.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.RemovePhoto;

public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IMapper _mapper;
    private IRepository<Playlist> _repository = null!;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var playlist = await _repository.GetWithTracksAndSubscribersAsync(id);
        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, id);
        }

        playlist.PhotoLocation = null;

        await _repository.UpdateAsync(playlist);

        var dto = _mapper.Map<Dto>(playlist);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Playlists.RemovePhoto, async (int id, IRepository<Playlist> repository) =>
        {
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}