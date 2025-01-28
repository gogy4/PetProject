using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;
[Route("profile")]
public class ProfileController : Controller
{
    private readonly EditUserService editUserService;

    public ProfileController(EditUserService editUserService)
    {
        this.editUserService = editUserService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Profile()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

        if (string.IsNullOrEmpty(currentUserId)) 
        {
            return RedirectToAction("Register", "Register");
        }

        var user = await editUserService.GetUser(currentUserId);

        if (user == null || !user.IsRegistrationComplete)
        {
            return RedirectToAction("Register", "Register"); 
        }

        return View(user);
    }


    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(UserEdit userEdit)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEdit.Id != currentUserId) return Forbid();  

        var user = await editUserService.EditUser(userEdit);
        return RedirectToAction("Profile", new { id = user.Id });
    }

    public async Task<IActionResult> Delete(UserEdit userEdit)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEdit.Id != currentUserId) return Forbid();

        await editUserService.DeleteUser(userEdit.Id);
        return RedirectToAction("", "Home");
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "Вы вышли из аккаунта";
        return RedirectToAction("Index", "Home");
    }
}
