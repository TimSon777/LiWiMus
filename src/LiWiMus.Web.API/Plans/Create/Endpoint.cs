using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Create;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Plan> _planRepository = null!;
    private readonly IMapper _mapper;
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

        var plan = _mapper.Map<Plan>(request);
        await _planRepository.AddAsync(plan);
        var dto = _mapper.Map<Dto>(plan);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Plans.Create, async (Request request, IRepository<Plan> planRepository) =>
        {
            _planRepository = planRepository;
            return await HandleAsync(request);
        });
    }
}