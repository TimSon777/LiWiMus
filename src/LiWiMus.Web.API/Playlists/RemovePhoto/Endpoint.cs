using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.RemovePhoto;

public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _repository;

    public Endpoint(IRepository<Playlist> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var playlist = await _repository.GetByIdAsync(id);
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
        app.MapPost(RouteConstants.Playlists.RemovePhoto, HandleAsync);
    }
}