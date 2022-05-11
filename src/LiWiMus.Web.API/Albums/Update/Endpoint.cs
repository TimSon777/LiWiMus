using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Albums;
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
    private readonly IRepository<Album> _albumRepository;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IRepository<Album> albumRepository, IMapper mapper)
    {
        _validator = validator;
        _albumRepository = albumRepository;
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

        _mapper.Map(request, album);

        await _albumRepository.UpdateAsync(album);
        
        var dto = _mapper.Map<Dto>(album);
        
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Albums.Update, HandleAsync);
    }
}