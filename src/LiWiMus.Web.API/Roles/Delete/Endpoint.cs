using LiWiMus.Core.Roles;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Roles.Delete;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Role> _repository = null!;

    public async Task<IResult> HandleAsync(int id)
    {
        var role = await _repository.GetByIdAsync(id);
        if (role is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Roles, id);
        }

        if (DefaultRoles.GetAll().Select(p => p.Name).Contains(role.Name))
        {
            return Results.BadRequest("Can't delete default role");
        }

        await _repository.DeleteAsync(role);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Roles.Delete, async (int id, IRepository<Role> repository) =>
        {
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}