using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

public class RegisterController : Controller
{
    private readonly RegisterService registerService;

    public RegisterController(RegisterService registerService)
    {
        this.registerService = registerService;
    }
    
    [Route("register")]
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [Route("register")]
    [ActionName("Register")]
    [HttpPost]
    public async Task<IActionResult> RegisterToDB(UserData userData)
    {
        var user = await registerService.SaveHashedPassword(userData);
        return RedirectToAction("Profile", "Profile", new { id = user.Id }); // передаем ID в маршрут
    }
}