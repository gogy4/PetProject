using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

[Route("profile")]
public class ProfileController : Controller
{
    private readonly PasteUserService pasteUserService;

    public ProfileController(PasteUserService pasteUserService)
    {
        this.pasteUserService = pasteUserService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Profile()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
 
        if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Register", "Register");
 
        var user =  await pasteUserService.GetUser(currentUserId);
        var pastes =  pasteUserService.GetPastesByUserId(currentUserId);
 
        if (user == null) return RedirectToAction("Register", "Register");
 
        var viewModel = new ProfileViewModel
        {
            User = new UserEdit(user),
            Pastes = pastes
        };
        return View(viewModel);
    }


    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(UserEdit userEdit)
    {
        var error = false;
        if (!string.IsNullOrEmpty(userEdit.NewPassword))
        {
            if (userEdit.NewPassword.Length < 8)
            {
                ModelState.AddModelError("Password", "Пароль должен быть длиннее 7 символов");
                error = true;
            }

            if (userEdit.NewPassword.ToLower() == userEdit.NewPassword ||
                userEdit.NewPassword.ToUpper() == userEdit.NewPassword)
            {
                ModelState.AddModelError("Password", "Символы в пароле должны быть разного регистра," +
                                                     " хотя бы один символ должен отличаться по регистру от других");
                error = true;
            }

            if (!Regex.IsMatch(userEdit.NewPassword, @"[!@#$%^&*?_\-+=]"))
            {
                ModelState.AddModelError("Password", "Пароль должен содержать хотя бы" +
                                                     " один из специальных символов !@#$%^&*?_\\-+=");
                error = true;
            }

            var user = await pasteUserService.GetUser(userEdit.Id);
            var passwordCorrect = pasteUserService.CheckPassword(userEdit.Password, user.Password);
            if (!passwordCorrect)
            {
                ModelState.AddModelError("Password", "Вы ввели неверный прошлый пароль");
                error = true;
            }
        }
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var pastes =  pasteUserService.GetPastesByUserId(currentUserId);

        var viewModel = new ProfileViewModel
        {
            User = userEdit,
            Pastes = pastes
        };
        if (error) return View("Profile", viewModel);
        await pasteUserService.EditUser(userEdit);
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> Delete(UserEdit userEdit)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEdit.Id != currentUserId) return Forbid();

        await pasteUserService.DeleteUser(userEdit.Id);
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