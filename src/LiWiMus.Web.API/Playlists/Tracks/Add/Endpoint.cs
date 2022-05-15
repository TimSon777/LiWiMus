using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.Tracks.Add;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Playlist> _playlistsRepository = null!;
    private IRepository<Track> _tracksRepository = null!;

    public async Task<IResult> HandleAsync(Request request)
    {
        var playlist = await _playlistsRepository.GetByIdAsync(request.PlaylistId);
        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, request.PlaylistId);
        }

        var track = await _tracksRepository.GetByIdAsync(request.TrackId);
        if (track is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Tracks, request.TrackId);
        }

        if (playlist.Tracks.Contains(track))
        {
            return Results.Conflict("The track is already in this playlist");
        }

        playlist.Tracks.Add(track);
        await _playlistsRepository.SaveChangesAsync();
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Playlists.Tracks.Add,
            async (Request request, IRepository<Playlist> playlistsRepository, IRepository<Track> tracksRepository) =>
            {
                _playlistsRepository = playlistsRepository;
                _tracksRepository = tracksRepository;
                return await HandleAsync(request);
            });
    }
}