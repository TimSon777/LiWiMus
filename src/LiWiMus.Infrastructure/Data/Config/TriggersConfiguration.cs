using EntityFrameworkCore.Triggers;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Transactions;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel;
using LiWiMus.SharedKernel.Interfaces;

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

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IAvatarService>(async entry =>
            await entry.Service.SetRandomAvatarAsync(entry.Entity));

        /*Triggers<User, ApplicationContext>.GlobalInserted.Add<UserManager<User>>(async entry =>
            await entry.Service.AddToRoleAsync(entry.Entity, DefaultRoles.User.Name));*/

        Triggers<User, ApplicationContext>.GlobalInserted.Add<IPlanManager>(async entry =>
            await entry.Service.AddToDefaultPlanAsync(entry.Entity));
    }
}