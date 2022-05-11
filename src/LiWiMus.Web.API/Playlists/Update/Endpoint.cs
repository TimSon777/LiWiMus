using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Playlists.Update;

public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IFormFileSaver _formFileSaver;
    private readonly IMapper _mapper;
    private readonly IRepository<Playlist> _repository;
    private readonly IValidator<Request> _validator;
    private readonly SharedSettings _settings;

    public Endpoint(IValidator<Request> validator, IRepository<Playlist> repository, IMapper mapper,
                    IFormFileSaver formFileSaver, IOptions<SharedSettings> settings)
    {
        _validator = validator;
        _repository = repository;
        _mapper = mapper;
        _formFileSaver = formFileSaver;
        _settings = settings.Value;
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
            return Results.UnprocessableEntity(new {detail = $"No playlists with Id {request.Id}."});
        }

        _mapper.Map(request, playlist);
        if (request.Photo is not null)
        {
            await UpdatePhoto(playlist, request.Photo);
        }
        
        await _repository.UpdateAsync(playlist);

        var dto = _mapper.Map<Dto>(playlist);
        return Results.Ok(dto);
    }

    private async Task UpdatePhoto(Playlist playlist, ImageFormFile photo)
    {
        if (playlist.PhotoPath is not null)
        {
            FileHelper.DeleteIfExists(Path.Combine(_settings.SharedDirectory, playlist.PhotoPath));
        }

        playlist.PhotoPath = await _formFileSaver.SaveWithRandomNameAsync(photo);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Playlists.Update, HandleAsync);
    }
}