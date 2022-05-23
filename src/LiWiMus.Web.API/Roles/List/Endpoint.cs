using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Roles.List;

public class Endpoint : IEndpoint<IResult>
{
    private IRepository<Role> _roleRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        var dto = _mapper.MapList<Role, Dto>(roles).ToList();
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Roles.List, async (IRepository<Role> roleRepository) =>
        {
            _roleRepository = roleRepository;
            return await HandleAsync();
        });
    }
}