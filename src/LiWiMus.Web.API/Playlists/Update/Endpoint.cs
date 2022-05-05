#region

using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;
using MinimalApi.Endpoint;

#endregion

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
        if (playlist.PhotoPath is not null)
        {
            FileHelper.DeleteIfExists(Path.Combine(_settings.SharedDirectory, playlist.PhotoPath));
        }

        playlist.PhotoPath = await _formFileSaver.SaveWithRandomNameAsync(request.Photo);

        await _repository.UpdateAsync(playlist);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/playlists", HandleAsync);
    }
}