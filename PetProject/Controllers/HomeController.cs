namespace PetProject.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}