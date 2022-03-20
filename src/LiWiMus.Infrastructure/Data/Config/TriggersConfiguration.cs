using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Entities;
using LiWiMus.SharedKernel;

namespace LiWiMus.Infrastructure.Data.Config;

public static class TriggersConfiguration
{
    public static void ConfigureTriggers()
    {
        Triggers<BaseEntity>.Inserting += entry => entry.Entity.CreatedAt = entry.Entity.ModifiedAt = DateTime.UtcNow;
        Triggers<BaseEntity>.Updating += entry => entry.Entity.ModifiedAt = DateTime.UtcNow;

        Triggers<BaseUserEntity>.Inserting += entry => entry.Entity.CreatedAt = entry.Entity.ModifiedAt = DateTime.UtcNow;
        Triggers<BaseUserEntity>.Updating += entry => entry.Entity.ModifiedAt = DateTime.UtcNow;

        Triggers<Transaction>.Inserting += entry => entry.Entity.User.Balance += entry.Entity.Amount;

        Triggers<User, ApplicationContext>.Inserted += entry => entry.Context.Transactions.Add(new Transaction
        {
            User = entry.Entity,
            Amount = 100,
            Description = "Gift for registration"
        });
    }
}