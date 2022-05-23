using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Permissions.List;

public class Endpoint : IEndpoint<IResult>
{
    private IRepository<Permission> _repository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync()
    {
        var permissions = await _repository.ListAsync();
        var dto = _mapper.MapList<Permission, Dto>(permissions);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Permissions.List, async (IRepository<Permission> repository) =>
        {
            _repository = repository;
            return await HandleAsync();
        });
    }
}