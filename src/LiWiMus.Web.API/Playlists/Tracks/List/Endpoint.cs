using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;
using LiWiMus.Core.Tracks.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;
using TrackDto = LiWiMus.Web.API.Tracks.Dto;

namespace LiWiMus.Web.API.Playlists.Tracks.List;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Playlist> _playlistRepository = null!;
    private IRepository<Track> _trackRepository = null!;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var playlist = await _playlistRepository.GetByIdAsync(request.PlaylistId);
        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, request.PlaylistId);
        }

        var tracks = await _trackRepository.ListInPlaylistAsync(playlist, request.Page, request.ItemsPerPage);
        var count = await _trackRepository.CountInPlaylistAsync(playlist);

        var trackDtos = _mapper.MapList<Track, TrackDto>(tracks);
        var result = new PaginatedData<TrackDto>(request.Page, request.ItemsPerPage, count, trackDtos);
        return Results.Ok(result);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Playlists.Tracks.List,
            async (int playlistId, int page, int itemsPerPage, IRepository<Playlist> playlistRepository,
                   IRepository<Track> trackRepository) =>
            {
                _playlistRepository = playlistRepository;
                _trackRepository = trackRepository;
                return await HandleAsync(new Request(playlistId, page, itemsPerPage));
            });
    }
}