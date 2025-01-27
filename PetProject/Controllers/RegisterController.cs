using Microsoft.AspNetCore.Mvc;

namespace PetProject.Controllers;

public class RegisterController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
}