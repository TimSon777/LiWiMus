using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.GetById;

public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _repository;

    public Endpoint(IMapper mapper, IRepository<Playlist> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var playlist = await _repository.GetByIdAsync(id);
        if (playlist is null)
        {
            return Results.NotFound();
        }

        var dto = _mapper.Map<Dto>(playlist);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Playlists.Read, HandleAsync);
    }
}