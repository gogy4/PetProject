using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

public class RegisterController(RegisterService registerService) : Controller
{
    private readonly CookieUtils cookieUtils = new();

    [Route("register")]
    [HttpGet]
    public IActionResult Register()
    {
        var currentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentId)) return View();
        TempData["Message"] = "Вы уже создали аккаунт, если хотите создать новый, выйдите из текущего";
        return RedirectToAction("index", "Home");
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> RegisterToDB(RegisterViewModel userData)
    {
        var error = await registerService.CheckCriteriaPassword(userData, userData.Password, ModelState);
        if (error) return View("Register", userData);

        var user = await registerService.SaveHashedPassword(userData);
        await cookieUtils.CreateCookie(user.Id, HttpContext);

        return RedirectToAction("Profile", "Profile");
    }
}