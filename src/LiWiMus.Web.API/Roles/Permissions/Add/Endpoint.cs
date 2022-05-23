using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Roles.Permissions.Add;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Role> _roleRepository = null!;
    private IRepository<SystemPermission> _permissionRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var role = await _roleRepository.GetWithPermissionsAsync(request.RoleId);
        if (role is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Roles, request.RoleId);
        }

        if (role.Permissions.Any(p => p.Id == request.PermissionId))
        {
            return Results.BadRequest("Role already has this permission");
        }

        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId);
        if (permission is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Permissions, request.PermissionId);
        }

        role.Permissions.Add(permission);
        await _roleRepository.UpdateAsync(role);

        var dto = _mapper.Map<Dto>(role);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Roles.SystemPermissions.Add,
            async (Request request, IRepository<Role> roleRepository,
                   IRepository<SystemPermission> permissionRepository) =>
            {
                _roleRepository = roleRepository;
                _permissionRepository = permissionRepository;
                return await HandleAsync(request);
            });
    }
}