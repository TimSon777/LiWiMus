using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.Extensions.Options;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.Update;

public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _repository;
    private readonly IValidator<Request> _validator;

    public Endpoint(IValidator<Request> validator, IRepository<Playlist> repository, IMapper mapper,
                    IOptions<SharedSettings> settings)
    {
        _validator = validator;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var playlist = await _repository.GetByIdAsync(request.Id);
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
        app.MapPatch(RouteConstants.Playlists.Update, HandleAsync);
    }
}