using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace LiWiMus.Web.Shared.Attributes;

public class AuthorizeWithJwtAttribute : AuthorizeAttribute
{
    public AuthorizeWithJwtAttribute()
    {
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    }
}