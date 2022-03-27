using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.SharedKernel;
using Microsoft.AspNetCore.Identity;

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

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IAvatarService>(entry =>
            entry.Service.SetRandomAvatarAsync(entry.Entity));

        Triggers<User, ApplicationContext>.GlobalInserted.Add<UserManager<User>>(entry =>
            entry.Service.AddToRoleAsync(entry.Entity, Roles.User.Name));
    }
}