using AutoMapper;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Tracks;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Tracks.Owners.List;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Track> _repository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var track = await _repository.GetByIdAsync(id);
        if (track is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Tracks, id);
        }

        var result = _mapper.MapList<Artist, Artists.Dto>(track.Owners);
        return Results.Ok(result);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Tracks.Owners.List, async (int id, IRepository<Track> repository) =>
        {
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}