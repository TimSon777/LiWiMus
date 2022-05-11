using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using Microsoft.Extensions.Options;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.RemovePhoto;

public class Endpoint : IEndpoint<IResult, int>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _repository;
    private readonly SharedSettings _settings;

    public Endpoint(IRepository<Playlist> repository, IOptions<SharedSettings> settings, IMapper mapper)
    {
        _repository = repository;
        _settings = settings.Value;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var playlist = await _repository.GetByIdAsync(id);
        if (playlist is null)
        {
            return Results.UnprocessableEntity(new {detail = $"No playlists with Id {id}."});
        }

        if (playlist.PhotoPath is not null)
        {
            FileHelper.DeleteIfExists(Path.Combine(_settings.SharedDirectory, playlist.PhotoPath));
            playlist.PhotoPath = null;
        }

        await _repository.UpdateAsync(playlist);

        var dto = _mapper.Map<Dto>(playlist);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Playlists.RemovePhoto, HandleAsync);
    }
}