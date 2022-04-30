using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace LiWiMus.API.Attributes;

public class AuthorizeWithJwtAttribute : AuthorizeAttribute
{
    public AuthorizeWithJwtAttribute()
    {
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    }
}