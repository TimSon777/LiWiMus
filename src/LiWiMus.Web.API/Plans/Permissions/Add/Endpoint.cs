using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Permissions.Add;

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

        if (plan.Permissions.Any(p => p.Id == request.PermissionId))
        {
            return Results.BadRequest("Plan already has this permission");
        }

        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId);
        if (permission is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Permissions, request.PermissionId);
        }

        plan.Permissions.Add(permission);
        await _planRepository.UpdateAsync(plan);

        var dto = _mapper.Map<Dto>(plan);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Plans.Permissions.Add,
            async (Request request, IRepository<Plan> planRepository, IRepository<Permission> permissionRepository) =>
            {
                _planRepository = planRepository;
                _permissionRepository = permissionRepository;
                return await HandleAsync(request);
            });
    }
}