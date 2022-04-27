namespace LiWiMus.API.Infrastructure.Attributes

open Microsoft.AspNetCore.Authorization
open OpenIddict.Validation.AspNetCore

type AuthorizeWithJwtAttribute() as this =
    inherit AuthorizeAttribute()
    do this.AuthenticationSchemes <- OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme