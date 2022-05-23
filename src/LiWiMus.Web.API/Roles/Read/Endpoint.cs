using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Roles.Read;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Role> _roleRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var role = await _roleRepository.GetWithPermissionsAsync(id);
        if (role is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Roles, id);
        }

        var dto = _mapper.Map<Dto>(role);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Roles.Read, async (int id, IRepository<Role> roleRepository) =>
        {
            _roleRepository = roleRepository;
            return await HandleAsync(id);
        });
    }
}