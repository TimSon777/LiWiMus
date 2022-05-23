using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Permissions.ReplaceAll;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Plan> _planRepository = null!;
    private IRepository<Permission> _permissionRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var plan = await _planRepository.GetWithPermissionsAsync(request.PlanId);
        if (plan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Plans, request.PlanId);
        }

        plan.Permissions.Clear();

        foreach (var id in request.Permissions)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission is null)
            {
                return Results.Extensions.NotFoundById(EntityType.Permissions, id);
            }

            plan.Permissions.Add(permission);
        }

        await _planRepository.UpdateAsync(plan);

        var dto = _mapper.Map<Dto>(plan);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut(RouteConstants.Plans.Permissions.ReplaceAll,
            async (Request request, IRepository<Plan> planRepository, IRepository<Permission> permissionRepository) =>
            {
                _planRepository = planRepository;
                _permissionRepository = permissionRepository;
                return await HandleAsync(request);
            });
    }
}