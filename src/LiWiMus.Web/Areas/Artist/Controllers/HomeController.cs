using LiWiMus.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.Artist.Controllers;

[Area("Artist")]
public class HomeController : Controller
{
    [Authorize(Policy = Permissions.Artist.View)]
    public IActionResult Index()
    {
        return Ok("you can see that");
    }
}