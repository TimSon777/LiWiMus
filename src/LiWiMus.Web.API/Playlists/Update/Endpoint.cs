using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Playlists.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.Update;

public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IMapper _mapper;
    private IRepository<Playlist> _repository = null!;
    private readonly IValidator<Request> _validator;

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

        var playlist = await _repository.GetWithTracksAndSubscribersAsync(request.Id);
        if (playlist is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Playlists, request.Id);
        }

        _mapper.Map(request, playlist);

        await _repository.UpdateAsync(playlist);

        var dto = _mapper.Map<Dto>(playlist);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Playlists.Update, async (Request request, IRepository<Playlist> repository) =>
        {
            _repository = repository;
            return await HandleAsync(request);
        });
    }
}