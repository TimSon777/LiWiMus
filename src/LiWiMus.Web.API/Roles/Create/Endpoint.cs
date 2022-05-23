using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Roles;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Roles.Create;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Role> _roleRepository = null!;
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

        var role = _mapper.Map<Role>(request);
        await _roleRepository.AddAsync(role);
        var dto = _mapper.Map<Dto>(role);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Roles.Create, async (Request request, IRepository<Role> roleRepository) =>
        {
            _roleRepository = roleRepository;
            return await HandleAsync(request);
        });
    }
}