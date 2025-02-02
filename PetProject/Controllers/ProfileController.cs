using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

[Route("profile")]
public class ProfileController(ProfileService profileService) : Controller
{
    private readonly CookieUtils cookieUtils = new();

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Profile()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
            return RedirectToAction("Register", "Register");

        var viewModel = await profileService.GetModel(User);
        return View(viewModel);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(ProfileViewModel model, string operation)
    {
        var userEdit = model.User;

        var error = await profileService.CheckCriteriaPassword(userEdit, userEdit.Password, ModelState,
            operation);

        var viewModel = await profileService.GetModel(User);
        if (error) return View("Profile", viewModel);
        await profileService.EditUser(userEdit);
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> DeleteUser(ProfileUserEditViewModel userEdit)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEdit.Id != currentUserId) return Forbid();
        await cookieUtils.DeleteCookie(HttpContext);
        TempData["Message"] = "Вы вышли из аккаунта";

        await profileService.DeleteUser(userEdit.Id);
        return RedirectToAction("", "Home");
    }

    [Route("DeleteAllPastes")]
    public async Task<IActionResult> DeleteAllPastes()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await profileService.DeleteAllPastes(currentUserId);
        return RedirectToAction("Profile");
    }

    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        await cookieUtils.DeleteCookie(HttpContext);
        TempData["Message"] = "Вы вышли из аккаунта";
        return RedirectToAction("Index", "Home");
    }

    [Route("DeletePaste")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await profileService.DeletePaste(id);
        return RedirectToAction("Profile");
    }
}