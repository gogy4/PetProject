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
    public async Task<IActionResult> RegisterToDB(UserData userData)
    {
        var error = false;
        if (userData.Password.Length < 8)
        {
            ModelState.AddModelError("Password", "Пароль должен быть длиннее 7 символов");
            error = true;
        }

        if (userData.Password.ToLower() == userData.Password || userData.Password.ToUpper() == userData.Password)
        {
            ModelState.AddModelError("Password", "Символы в пароле должны быть разного регистра," +
                                                 " хотя бы один символ должен отличаться по регистру от других");
            error = true;
        }

        if (!Regex.IsMatch(userData.Password, @"[!@#$%^&*?_\-+=]"))
        {
            ModelState.AddModelError("Password", "Пароль должен содержать хотя бы" +
                                                 " один из специальных символов !@#$%^&*?_\\-+=");
            error = true;
        }

        if (error) return View("Register", userData);

        var user = await registerService.SaveHashedPassword(userData);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Name)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Profile", "Profile");
    }
}