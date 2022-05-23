using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Albums.Specifications;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Update;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private IRepository<Album> _albumRepository = null!;
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

        var album = await _albumRepository.GetByIdAsync(request.Id);

        if (album is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Albums, request.Id);
        }

        if (request.PublishedAt is not null && DateOnly.FromDateTime(album.CreatedAt) < request.PublishedAt.Value)
        {
            return Results.Conflict(new { detail = "Publication date must be less than created date" });
        }

        _mapper.Map(request, album);

        await _albumRepository.UpdateAsync(album);

        var dto = _mapper.Map<Dto>(album);
        dto.Artists = _mapper.MapList<Artist, Artists.Dto>(await _albumRepository.GetArtistsAsync(album)).ToList();
        dto.TracksCount = await _albumRepository.GetTracksCountAsync(album);
        dto.ListenersCount = await _albumRepository.GetListenersCountAsync(album);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Albums.Update, async (Request request, IRepository<Album> repository) =>
        {
            _albumRepository = repository;
            return await HandleAsync(request);
        });
    }
}