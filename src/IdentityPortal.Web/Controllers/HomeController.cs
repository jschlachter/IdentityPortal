using Microsoft.AspNetCore.Mvc;

namespace IdentityPortal.Controllers;

public class HomeController : Controller
{
    readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Content("Hello World!");
    }
}
