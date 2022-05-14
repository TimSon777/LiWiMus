using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;
using TrackDto = LiWiMus.Web.API.Tracks.Dto;

namespace LiWiMus.Web.API.Playlists.Tracks.List;

public class Endpoint : IEndpoint<IResult, int, int, int>
{
    private readonly IRepository<Playlist> _repository;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IRepository<Playlist> repository, IValidator<Request> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int playlistId, int page, int itemsPerPage)
    {
        var request = new Request(playlistId, page, itemsPerPage);
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var playlist = await _repository.GetByIdAsync(request.PlaylistId);
        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, request.PlaylistId);
        }

        var tracks = playlist.PlaylistTracks
                             .OrderBy(pt => pt.CreatedAt)
                             .Skip((request.Page - 1) * request.ItemsPerPage)
                             .Take(request.ItemsPerPage)
                             .Select(pt => pt.Track)
                             .ToList();

        var trackDtos = _mapper.MapList<Track, TrackDto>(tracks);
        var result = new PaginatedData<TrackDto>(page, itemsPerPage, playlist.Tracks.Count, trackDtos);
        return Results.Ok(result);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Playlists.Tracks.List, HandleAsync);
    }
}