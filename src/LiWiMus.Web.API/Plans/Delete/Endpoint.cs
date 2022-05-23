using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Delete;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Plan> _repository = null!;

    public async Task<IResult> HandleAsync(int id)
    {
        var plan = await _repository.GetByIdAsync(id);
        if (plan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Plans, id);
        }

        if (DefaultPlans.GetAll().Select(p => p.Name).Contains(plan.Name))
        {
            return Results.BadRequest("Can't delete default plan");
        }

        await _repository.DeleteAsync(plan);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Plans.Delete, async (int id, IRepository<Plan> repository) =>
        {
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}