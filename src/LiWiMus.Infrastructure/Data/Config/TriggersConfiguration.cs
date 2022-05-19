using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Transactions;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel;
using LiWiMus.SharedKernel.Interfaces;
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

        Triggers<Transaction>.GlobalInserting.Add<IRepository<User>>(async entry =>
        {
            var transaction = entry.Entity;
            var userId = transaction.UserId;
            var user = await entry.Service.GetByIdAsync(userId);
            if (user != null)
            {
                user.Balance += transaction.Amount;
            }
        });

        Triggers<User, ApplicationContext>.Inserted += entry => entry.Context.Transactions.Add(new Transaction
        {
            User = entry.Entity,
            Amount = 100,
            Description = "Gift for registration"
        });

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IAvatarService>(entry =>
            entry.Service.SetRandomAvatarAsync(entry.Entity));

        Triggers<User, ApplicationContext>.GlobalInserted.Add<UserManager<User>>(entry =>
            entry.Service.AddToRoleAsync(entry.Entity, DefaultRoles.User.Name));

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IUserPlanManager>(entry =>
            entry.Service.AddToPlan(entry.Entity, DefaultPlans.Free, TimeSpan.FromDays(30)));
    }
}