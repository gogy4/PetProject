using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

public class LogInController : Controller
{
    private LogInService logInService;

    public LogInController(LogInService logInService)
    {
        this.logInService = logInService;
    }
    
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
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }
        TempData["error"] = "Вы ввели неверные данные";
        return RedirectToAction("Login");
    }
    
}