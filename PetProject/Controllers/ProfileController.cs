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
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        this._profileService = profileService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Profile()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
            return RedirectToAction("Register", "Register");

        var user = await _profileService.GetUser(currentUserId);
        var pastes = _profileService.GetPastesByUserId(currentUserId);

        if (user == null)
            return RedirectToAction("Register", "Register");

        var viewModel = new ProfileViewModel
        {
            User = new ProfileUserEditViewModel(user),
            Pastes = pastes
        };
        return View(viewModel);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(ProfileViewModel model)
    {
        var error = false;
        var userEdit = model.User;
        
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

            var user = await _profileService.GetUser(userEdit.Id);
            var passwordCorrect = _profileService.CheckPassword(userEdit.Password, user.Password);
            if (!passwordCorrect)
            {
                ModelState.AddModelError("Password", "Вы ввели неверный прошлый пароль");
                error = true;
            }
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var pastes = _profileService.GetPastesByUserId(currentUserId);

        var viewModel = new ProfileViewModel
        {
            User = new ProfileUserEditViewModel(await _profileService.GetUser(currentUserId)),
            Pastes = pastes
        };
        if (error)
        {
            return View("Profile", viewModel);
        }
        await _profileService.EditUser(userEdit);
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> Delete(ProfileUserEditViewModel userEdit)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEdit.Id != currentUserId) return Forbid();

        await _profileService.DeleteUser(userEdit.Id);
        return RedirectToAction("", "Home");
    }
    [Route("deleteallpastes")]
    public async Task<IActionResult> DeleteAllPastes()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _profileService.DeleteAllPastes(currentUserId);
        return RedirectToAction("Profile");
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        foreach (var cookie in HttpContext.Request.Cookies.Keys) HttpContext.Response.Cookies.Delete(cookie);

        HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        TempData["Message"] = "Вы вышли из аккаунта";

        return RedirectToAction("Index", "Home");
    }
    [Route("DeletePaste")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _profileService.DeletePaste(id);
        return RedirectToAction("Profile");
    }
}