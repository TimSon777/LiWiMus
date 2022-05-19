using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Transactions;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Transactions.Update;

public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IMapper _mapper;
    private IRepository<Transaction> _repository = null!;
    private readonly IValidator<Request> _validator;

    public Endpoint(IMapper mapper, IValidator<Request> validator)
    {
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var transaction = await _repository.GetByIdAsync(request.Id);

        if (transaction is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Transactions, request.Id);
        }

        _mapper.Map(request, transaction);
        
        await _repository.UpdateAsync(transaction);

        var dto = _mapper.Map<Dto>(transaction);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.Transactions.Update, async (Request request, IRepository<Transaction> repository) =>
        {
            _repository = repository;
            return await HandleAsync(request);
        });
    }
}