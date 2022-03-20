using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class TransactionsByUserIdSpecification : Specification<Transaction>
{
    public TransactionsByUserIdSpecification(int userId)
    {
        Query.Where(transaction => transaction.User.Id == userId);
    }
}