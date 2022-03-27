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
    private readonly RoleManager<Role> _roleManager;

    public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
                                  IPasswordHasher<User> passwordHasher,
                                  IEnumerable<IUserValidator<User>> userValidators,
                                  IEnumerable<IPasswordValidator<User>> passwordValidators,
                                  ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                  IServiceProvider services, ILogger<ApplicationUserManager> logger,
                                  IRepository<Role> roleRepository,
                                  RoleManager<Role> roleManager) : base(
        store, optionsAccessor, passwordHasher,
        userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _roleRepository = roleRepository;
        _roleManager = roleManager;
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