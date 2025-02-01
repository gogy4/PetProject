using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

using PetProject.Services;

namespace PetProject.Controllers;

public class LogInController(LogInService logInService) : Controller
{
    private readonly CookieUtils cookieUtils = new ();
    [HttpGet]
    [Route("/login")]
    public async Task<IActionResult> LogIn()
    {
        var currentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentId)) return View();
        TempData["Message"] = "Вы уже вошли аккаунт, если хотите войти в новый, выйдите из текущего";
        return RedirectToAction("index", "Home");
    }
    
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> LogIn(string email, string password)
    {
        var user = await logInService.GetUser(email);
        if (user is null)
        {
            TempData["error"] = "Пользователь с такой почтой не найден";
            return RedirectToAction("Login");
        }
        var logIn = await logInService.CheckPassword(user, password);
        if (logIn)
        {
            await cookieUtils.CreateCookie(user.Id, HttpContext);
            return RedirectToAction("Profile", "Profile");
        }
        TempData["error"] = "Вы ввели неверные данные";
        return RedirectToAction("Login");
    }
    
}