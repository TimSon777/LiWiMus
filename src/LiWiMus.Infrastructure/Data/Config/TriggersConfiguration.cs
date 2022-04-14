using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Transactions;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Identity;
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

        Triggers<User, ApplicationContext>.Inserted += async entry =>
        {
            entry.Context.Transactions.Add(new Transaction
            {
                User = entry.Entity,
                Amount = 100,
                Description = "Gift for registration"
            });
            await entry.Context.SaveChangesAsync();
        };

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IAvatarService>(async entry =>
        {
            await entry.Service.SetRandomAvatarAsync(entry.Entity);
            await entry.Context.SaveChangesAsync();
        });

        Triggers<UserIdentity, ApplicationContext>.GlobalInserted.Add<UserManager<UserIdentity>>(entry =>
            entry.Service.AddToRoleAsync(entry.Entity, Roles.User.Name));
    }
}