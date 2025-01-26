using Microsoft.AspNetCore.Mvc;

namespace PetProject.Areas.Account.Controllers;
[Area("Account")]
public class HomeController : Controller
{
    [Route("{area}")]
    public IActionResult Index()
    {
        return View();
    }
}