using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Exceptions;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Plans.Delete;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<Plan> _repository = null!;
    private IPlanManager _planManager = null!;

    public async Task<IResult> HandleAsync(int id)
    {
        var plan = await _repository.GetByIdAsync(id);
        if (plan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Plans, id);
        }

        try
        {
            await _planManager.DeleteAsync(plan);
        }
        catch (DeleteDefaultPlanException)
        {
            return Results.UnprocessableEntity("Can't delete default plan");
        }

        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Plans.Delete,
            async (int id, IRepository<Plan> repository, IPlanManager planManager) =>
        {
            _planManager = planManager;
            _repository = repository;
            return await HandleAsync(id);
        });
    }
}