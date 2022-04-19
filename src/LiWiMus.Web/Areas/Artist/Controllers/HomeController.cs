using LiWiMus.Core.Constants;
using LiWiMus.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.Artist.Controllers;

[Area("Artist")]
public class HomeController : Controller
{
    [Authorize(Policy = DefaultPermissions.Artist.Read)]
    public IActionResult Index()
    {
        return Ok("you can see that");
    }
}