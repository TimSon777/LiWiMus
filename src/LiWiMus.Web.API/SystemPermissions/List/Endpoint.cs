using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.SystemPermissions.List;

public class Endpoint : IEndpoint<IResult>
{
    private IRepository<SystemPermission> _repository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync()
    {
        var permissions = await _repository.ListAsync();
        var dto = _mapper.MapList<SystemPermission, Dto>(permissions);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.SystemPermissions.List, async (IRepository<SystemPermission> repository) =>
        {
            _repository = repository;
            return await HandleAsync();
        });
    }
}