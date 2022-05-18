using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Genres;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Genres.Create;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Genre> _genreRepository = null!;
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

        var genre = _mapper.Map<Genre>(request);
        await _genreRepository.AddAsync(genre);
        var dto = _mapper.Map<Dto>(genre);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Genres.Create, async (Request request, IRepository<Genre> genreRepository) =>
        {
            _genreRepository = genreRepository;
            return await HandleAsync(request);
        });
    }
}