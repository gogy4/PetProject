using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

public class RegisterController : Controller
{
    private readonly RegisterService registerService;
    private readonly CookieUtils cookieUtils = new();

    public RegisterController(RegisterService registerService)
    {
        this.registerService = registerService;
    }

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