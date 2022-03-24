using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LiWiMus.Web.Permission;

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
        {
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new PermissionRequirement(policyName));
        return Task.FromResult(policy.Build())!;
    }
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
}