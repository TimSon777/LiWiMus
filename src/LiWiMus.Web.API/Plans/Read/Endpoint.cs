using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Read;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Plan> _planRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var plan = await _planRepository.GetWithPermissionsAsync(id);
        if (plan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Plans, id);
        }

        var dto = _mapper.Map<Dto>(plan);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Plans.Read, async (int id, IRepository<Plan> planRepository) =>
        {
            _planRepository = planRepository;
            return await HandleAsync(id);
        });
    }
}