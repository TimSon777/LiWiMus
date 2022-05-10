using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Read;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IRepository<Album> _albumRepository;
    private readonly IMapper _mapper;

    public Endpoint(IRepository<Album> albumRepository, IMapper mapper)
    {
        _albumRepository = albumRepository;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var album = await _albumRepository.GetByIdAsync(id);

        if (album is null)
        {
            return Results.UnprocessableEntity(new { detail = $"No albums with Id {id}." });
        }

        var dto = _mapper.Map<Dto>(album);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Albums.Read, HandleAsync);
    }
}