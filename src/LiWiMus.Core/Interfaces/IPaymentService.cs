using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Interfaces;

public interface IPaymentService
{
    Task PayAsync(User user, decimal amount, string? reason = null);
}