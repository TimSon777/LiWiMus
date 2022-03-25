using System.Security.Claims;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Models;
using LiWiMus.Core.Specifications;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Packaging;

namespace LiWiMus.Infrastructure.Services;

public class ApplicationUserManager : UserManager<User>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<UserClaim> _userClaimRepository;
    private readonly IdentityErrorDescriber _errorDescriber = new();
    private readonly RoleManager<Role> _roleManager;

    public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
                                  IPasswordHasher<User> passwordHasher,
                                  IEnumerable<IUserValidator<User>> userValidators,
                                  IEnumerable<IPasswordValidator<User>> passwordValidators,
                                  ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                  IServiceProvider services, ILogger<ApplicationUserManager> logger,
                                  IRepository<Role> roleRepository, IRepository<UserRole> userRoleRepository,
                                  RoleManager<Role> roleManager, IRepository<UserClaim> userClaimRepository) : base(
        store, optionsAccessor, passwordHasher,
        userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _roleManager = roleManager;
        _userClaimRepository = userClaimRepository;
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
        if (await IsInRoleAsync(user, roleName))
        {
            return IdentityResult.Failed(_errorDescriber.UserAlreadyInRole(roleName));
        }

        var role = await _roleRepository.GetBySpecAsync(new RoleByNameSpec(roleName));

        if (role is null)
        {
            return IdentityResult.Failed(_errorDescriber.InvalidRoleName(roleName));
        }

        var existingUserRole = await _userRoleRepository.GetBySpecAsync(new UserRoleByUserAndRoleSpec(user, role));
        if (existingUserRole is not null)
        {
            await _userRoleRepository.DeleteAsync(existingUserRole);
        }

        var userRole = new UserRole
        {
            User = user,
            Role = role,
            GrantedAt = DateTime.UtcNow,
            ActiveUntil = DateTime.UtcNow.AddOrMaximize(role.DefaultTimeout)
        };
        await _userRoleRepository.AddAsync(userRole);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
    {
        var errors = new List<IdentityError>();
        foreach (var role in roles)
        {
            var result = await AddToRoleAsync(user, role);
            errors.AddRange(result.Errors);
        }

        return errors.Count > 0
            ? IdentityResult.Failed(errors.ToArray())
            : IdentityResult.Success;
    }

    public override async Task<bool> IsInRoleAsync(User user, string roleName)
    {
        var role = await _roleRepository.GetBySpecAsync(new RoleByNameSpec(roleName));

        if (role is null)
        {
            return false;
        }

        var userRole = await _userRoleRepository.GetBySpecAsync(new UserRoleByUserAndRoleSpec(user, role));
        return userRole is not null && userRole.IsActive;
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

    public override async Task<IdentityResult> AddClaimAsync(User user, Claim claim)
    {
        var existingUserClaim = await _userClaimRepository.GetBySpecAsync(new UserClaimByUserAndClaimSpec(user, claim));
        if (existingUserClaim is not null)
        {
            await _userClaimRepository.DeleteAsync(existingUserClaim);
        }

        var timeLimitedClaim = claim as TimeLimitedClaim;
        timeLimitedClaim ??= new TimeLimitedClaim(claim.Type, claim.Value);

        var userClaim = new UserClaim
        {
            UserId = user.Id,
            ClaimType = timeLimitedClaim.Type,
            ClaimValue = timeLimitedClaim.Value,
            GrantedAt = timeLimitedClaim.GrantedAt,
            ActiveUntil = timeLimitedClaim.ActiveUntil
        };

        await _userClaimRepository.AddAsync(userClaim);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims)
    {
        var errors = new List<IdentityError>();
        foreach (var claim in claims)
        {
            var result = await AddClaimAsync(user, claim);
            errors.AddRange(result.Errors);
        }

        return errors.Count > 0
            ? IdentityResult.Failed(errors.ToArray())
            : IdentityResult.Success;
    }

    public override async Task<IList<Claim>> GetClaimsAsync(User user)
    {
        return await _userClaimRepository.ListAsync(new ActiveClaimsByUserSpec(user));
    }

    public async Task<IList<Claim>> GetAllClaimsAsync(User user)
    {
        var userClaims = await GetClaimsAsync(user);
        var userRoleNames = await GetRolesAsync(user);
        var userRoles = await _roleRepository.ListAsync(new RolesByNamesSpec(userRoleNames));
        foreach (var role in userRoles)
        {
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            userClaims.AddRange(roleClaims);
        }

        return userClaims;
    }
}