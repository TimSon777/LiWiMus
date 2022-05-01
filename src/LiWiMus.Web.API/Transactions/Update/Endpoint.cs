using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Transactions;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Transactions.Update;

public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Transaction> _repository;
    private readonly IValidator<Request> _validator;

    public Endpoint(IMapper mapper, IValidator<Request> validator, IRepository<Transaction> repository)
    {
        _mapper = mapper;
        _validator = validator;
        _repository = repository;
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
            return Results.UnprocessableEntity(new {detail = $"No transactions with Id {request.Id}."});
        }

        _mapper.Map(request, transaction);
        await _repository.UpdateAsync(transaction);
        return Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/transactions", HandleAsync);
    }
}