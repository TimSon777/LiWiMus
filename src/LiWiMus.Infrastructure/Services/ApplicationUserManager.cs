using System.Security.Claims;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Services;

public class ApplicationUserManager : UserManager<User>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserRole> _userRoleRepository;

    public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
                                  IPasswordHasher<User> passwordHasher,
                                  IEnumerable<IUserValidator<User>> userValidators,
                                  IEnumerable<IPasswordValidator<User>> passwordValidators,
                                  ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                  IServiceProvider services, ILogger<ApplicationUserManager> logger,
                                  IRepository<Role> roleRepository, IRepository<UserRole> userRoleRepository) : base(
        store, optionsAccessor, passwordHasher,
        userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    public override async Task<IdentityResult> CreateAsync(User user)
    {
        var result = await base.CreateAsync(user);

        if (result.Succeeded)
        {
            result = await AddToRoleAsync(user, Roles.User.Name);
        }

        return result;
    }

    public override async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _roleRepository.ListAsync(new NamesOfActiveRolesByUserAsyncSpec(user));
    }

    public override async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
    {
        var role = await _roleRepository.GetBySpecAsync(new RoleByNameSpec(roleName));

        if (role is null)
        {
            return IdentityResult.Failed(new IdentityErrorDescriber().InvalidRoleName(roleName));
        }

        if (await IsInRoleAsync(user, roleName))
        {
            return IdentityResult.Failed(new IdentityErrorDescriber().UserAlreadyInRole(roleName));
        }

        var userRole = new UserRole
        {
            User = user,
            Role = role,
            GrantedAt = DateTime.Now,
            ActiveUntil = DateTime.MaxValue
        };
        await _userRoleRepository.AddAsync(userRole);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
    {
        var tasks = roles.Select(role => AddToRoleAsync(user, role));
        var results = await Task.WhenAll(tasks);
        return results.Any(result => !result.Succeeded)
            ? IdentityResult.Failed(results.SelectMany(result => result.Errors).ToArray())
            : IdentityResult.Success;
    }

    public override async Task<bool> IsInRoleAsync(User user, string role)
    {
        var roles = await GetRolesAsync(user);
        return roles.Contains(role);
    }

    public override Task<IList<User>> GetUsersForClaimAsync(Claim claim)
    {
        return base.GetUsersForClaimAsync(claim);
    }

    public override Task<IList<User>> GetUsersInRoleAsync(string roleName)
    {
        return base.GetUsersInRoleAsync(roleName);
    }

    public override Task<IdentityResult> RemoveClaimAsync(User user, Claim claim)
    {
        return base.RemoveClaimAsync(user, claim);
    }

    public override Task<IdentityResult> RemoveClaimsAsync(User user, IEnumerable<Claim> claims)
    {
        return base.RemoveClaimsAsync(user, claims);
    }

    public override Task<IdentityResult> ReplaceClaimAsync(User user, Claim claim, Claim newClaim)
    {
        return base.ReplaceClaimAsync(user, claim, newClaim);
    }

    public override Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
    {
        return base.RemoveFromRoleAsync(user, role);
    }

    public override Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
    {
        return base.RemoveFromRolesAsync(user, roles);
    }

    public override Task<IdentityResult> AddClaimAsync(User user, Claim claim)
    {
        return base.AddClaimAsync(user, claim);
    }

    public override Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims)
    {
        return base.AddClaimsAsync(user, claims);
    }

    public override Task<IList<Claim>> GetClaimsAsync(User user)
    {
        return base.GetClaimsAsync(user);
    }
}