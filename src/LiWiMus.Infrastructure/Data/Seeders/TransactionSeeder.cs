using LiWiMus.Core.Transactions;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Enums;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class TransactionSeeder : ISeeder
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Transaction> _transactionRepository;

    public TransactionSeeder(UserManager<User> userManager, IRepository<Transaction> transactionRepository)
    {
        _userManager = userManager;
        _transactionRepository = transactionRepository;
    }
    
    public async Task SeedAsync(EnvironmentType environmentType)
    {
        const string userName = "MockUser_Trans";
        if (await _userManager.FindByNameAsync(userName) is not null)
        {
            return;
        }
        
        switch (environmentType)
        {
            case EnvironmentType.Development:
            case EnvironmentType.Testing:
                var user = new User
                {
                    Id = 40000,
                    UserName = userName,
                    Gender = Gender.Male,
                    AvatarLocation = "Location",
                    Email = "mockEmail@mock.mock_Trans"
                };

                await _userManager.CreateAsync(user, "Password");
                await _userManager.UpdateAsync(user);
                var transaction = new Transaction
                {
                    Id = 1000,
                    Amount = -100,
                    Description = "Description",
                    User = user
                };

                await _transactionRepository.AddAsync(transaction);
                break;
            case EnvironmentType.Production:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
        }
    }

    public int Priority => 20;
}