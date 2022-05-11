using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Albums.Create;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;
    private readonly IRepository<Album> _albumRepository;
    private readonly IFormFileSaver _formFileSaver;
    private readonly IRepository<Artist> _artistRepository;

    public Endpoint(IValidator<Request> validator, IMapper mapper, IRepository<Album> albumRepository, 
        IFormFileSaver formFileSaver, IRepository<Artist> artistRepository)
    {
        _validator = validator;
        _mapper = mapper;
        _albumRepository = albumRepository;
        _formFileSaver = formFileSaver;
        _artistRepository = artistRepository;
    }
    
    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var album = _mapper.Map<Album>(request);
        album.CoverLocation = await _formFileSaver.SaveWithRandomNameAsync(request.Cover);
        album.Owners = new List<Artist>();
        
        foreach (var artistId in request.ArtistIds)
        {
            var artist = await _artistRepository.GetByIdAsync(artistId);
            
            if (artist is null)
            {
                return Results.UnprocessableEntity(new {detail = $"No artists with Id {artistId}."});
            }
            
            album.Owners.Add(artist);
        }

        await _albumRepository.AddAsync(album);
        
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Albums.Create, HandleAsync);
    }
}