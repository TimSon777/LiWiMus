using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Transactions;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Transactions.Create;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<Transaction> _transactionRepository = null!;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var transaction = _mapper.Map<Transaction>(request);
        await _transactionRepository.AddAsync(transaction);

        var dto = _mapper.Map<Dto>(transaction);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Transactions.Create,
            async (Request request, IRepository<Transaction> transactionRepository) =>
            {
                _transactionRepository = transactionRepository;
                return await HandleAsync(request);
            });
    }
}