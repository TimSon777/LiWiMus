using LiWiMus.Core.Roles;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class UserSeeder : ISeeder
{
    private readonly UserManager<User> _userManager;
    private readonly AdminSettings _adminSettings;

    public UserSeeder(UserManager<User> userManager, IOptions<AdminSettings> adminSettingsOptions)
    {
        _userManager = userManager;
        _adminSettings = adminSettingsOptions.Value;
    }
    public async Task SeedAsync(EnvironmentType environmentType)
    {
        await SeedAdminAsync();
    }
    
    private async Task SeedAdminAsync()
    {
        var oldAdmin = await _userManager.FindByNameAsync(_adminSettings.UserName);
        if (oldAdmin is not null)
        {
            return;
        }
        
        var admin = new User
        {
            UserName = _adminSettings.UserName,
            Email = _adminSettings.Email,
            EmailConfirmed = true
        };
        
        var user = await _userManager.FindByEmailAsync(admin.Email);
        
        if (user == null)
        {
            await _userManager.CreateAsync(admin, _adminSettings.Password);
        }
        else
        {
            admin = user;
        }
        
        await _userManager.UpdateAsync(admin);
        await _userManager.AddToRoleAsync(admin, DefaultRoles.Admin.Name);
    }

    public int Priority => 80;
}