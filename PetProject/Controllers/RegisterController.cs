using System.Security.Claims;
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
        return View();
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> RegisterToDB(UserData userData)
    {
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