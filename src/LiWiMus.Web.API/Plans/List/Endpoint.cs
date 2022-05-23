using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.List;

public class Endpoint : IEndpoint<IResult>
{
    private IRepository<Plan> _planRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync()
    {
        var plans = await _planRepository.GetAllAsync();
        var dto = _mapper.MapList<Plan, Dto>(plans).ToList();
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Plans.List, async (IRepository<Plan> planRepository) =>
        {
            _planRepository = planRepository;
            return await HandleAsync();
        });
    }
}