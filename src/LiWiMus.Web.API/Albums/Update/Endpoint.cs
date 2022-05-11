using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.Extensions.Options;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Update;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private readonly IRepository<Album> _albumRepository;
    private readonly IMapper _mapper;
    private readonly SharedSettings _settings;
    private readonly IFormFileSaver _formFileSaver;

    public Endpoint(IValidator<Request> validator, IRepository<Album> albumRepository, IMapper mapper, IOptions<SharedSettings> sharedSettingsOpt, IFormFileSaver formFileSaver)
    {
        _validator = validator;
        _albumRepository = albumRepository;
        _mapper = mapper;
        _formFileSaver = formFileSaver;
        _settings = sharedSettingsOpt.Value;
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
            return Results.UnprocessableEntity(new { detail = $"No albums with Id {request.Id}." });
        }

        _mapper.Map(request, album);
        
        FileHelper.DeleteIfExists(Path.Combine(_settings.SharedDirectory, album.CoverLocation));

        album.CoverLocation = await _formFileSaver.SaveWithRandomNameAsync(request.Cover);
        
        await _albumRepository.UpdateAsync(album);
        
        var dto = _mapper.Map<Dto>(album);
        
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Albums.Update, HandleAsync);
    }
}