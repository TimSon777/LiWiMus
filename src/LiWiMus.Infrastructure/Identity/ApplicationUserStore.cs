using System.Security.Claims;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Models;
using LiWiMus.Core.Specifications;
using LiWiMus.Infrastructure.Data;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LiWiMus.Infrastructure.Identity;

public class ApplicationUserStore :
    UserStore<User, Role, ApplicationContext, int, UserClaim, UserRole, IdentityUserLogin<int>, IdentityUserToken<int>,
        IdentityRoleClaim<int>>, IUserStore<User>, IUserRoleStore<User>, IUserClaimStore<User>,
    IUserPasswordStore<User>, IUserSecurityStampStore<User>, IUserEmailStore<User>,
    IUserPhoneNumberStore<User>, IQueryableUserStore<User>, IUserLoginStore<User>,
    IUserTwoFactorStore<User>, IUserLockoutStore<User>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<UserClaim> _userClaimRepository;

    public ApplicationUserStore(ApplicationContext context, IRepository<Role> roleRepository,
                                IRepository<UserRole> userRoleRepository, IRepository<UserClaim> userClaimRepository,
                                IdentityErrorDescriber describer) : base(context, describer)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _userClaimRepository = userClaimRepository;
    }

    public override async Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName,
                                                                CancellationToken cancellationToken = new())
    {
        var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
        if (role is null)
        {
            return new List<User>();
        }

        return await _userRoleRepository.ListAsync(new UsersByActiveRoleSpec(role), cancellationToken);
    }

    public override async Task<IList<User>> GetUsersForClaimAsync(Claim claim,
                                                                  CancellationToken cancellationToken = new())
    {
        return await _userClaimRepository.ListAsync(new UsersByActiveClaimSpec(claim), cancellationToken);
    }

    public override async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = new())
    {
        return await _userClaimRepository.ListAsync(new ActiveClaimsByUserSpec(user), cancellationToken);
    }

    public override async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = new())
    {
        return await _roleRepository.ListAsync(new NamesOfActiveRolesByUserAsyncSpec(user), cancellationToken);
    }

    public override async Task<bool> IsInRoleAsync(User user, string normalizedRoleName,
                                                   CancellationToken cancellationToken = new())
    {
        var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

        if (role is null)
        {
            return false;
        }

        var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
        return userRole is {IsActive: true};
    }

    protected override UserRole CreateUserRole(User user, Role role)
    {
        return new UserRole
        {
            User = user,
            Role = role,
            GrantedAt = DateTime.UtcNow,
            ActiveUntil = DateTime.UtcNow.AddOrMaximize(role.DefaultTimeout)
        };
    }

    public override async Task AddToRoleAsync(User user, string normalizedRoleName,
                                              CancellationToken cancellationToken = new())
    {
        await TryDeleteUserRoleAsync(user, normalizedRoleName, cancellationToken);
        await base.AddToRoleAsync(user, normalizedRoleName, cancellationToken);
    }

    public override async Task AddClaimsAsync(User user, IEnumerable<Claim> claims,
                                              CancellationToken cancellationToken = new CancellationToken())
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        foreach (var claim in claims)
        {
            await TryDeleteUserClaimAsync(user, claim, cancellationToken);
            await _userClaimRepository.AddAsync(CreateUserClaim(user, claim), cancellationToken);
        }
    }

    private async Task<bool> TryDeleteUserClaimAsync(User user, Claim claim,
                                                     CancellationToken cancellationToken = new())
    {
        var userClaim =
            await _userClaimRepository.GetBySpecAsync(new UserClaimByUserAndClaimSpec(user, claim), cancellationToken);
        if (userClaim is null)
        {
            return false;
        }

        await _userClaimRepository.DeleteAsync(userClaim, cancellationToken);
        return true;
    }

    protected override UserClaim CreateUserClaim(User user, Claim claim)
    {
        var timeLimitedClaim = claim as TimeLimitedClaim;
        timeLimitedClaim ??= new TimeLimitedClaim(claim.Type, claim.Value);

        return new UserClaim
        {
            UserId = user.Id,
            ClaimType = timeLimitedClaim.Type,
            ClaimValue = timeLimitedClaim.Value,
            GrantedAt = timeLimitedClaim.GrantedAt,
            ActiveUntil = timeLimitedClaim.ActiveUntil
        };
    }

    private async Task<bool> TryDeleteUserRoleAsync(User user, string normalizedRoleName,
                                                    CancellationToken cancellationToken = new())
    {
        var userRole =
            await _userRoleRepository.GetBySpecAsync(
                new UserRoleByUserAndNormalizedRoleNameSpec(user, normalizedRoleName),
                cancellationToken);

        if (userRole is null)
        {
            return false;
        }

        await _userRoleRepository.DeleteAsync(userRole, cancellationToken);
        return true;
    }
}