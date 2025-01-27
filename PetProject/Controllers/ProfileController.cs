using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;
[Route("profile")]
public class ProfileController : Controller
{
    private EditUserService editUserService;

    public ProfileController(EditUserService editUserService)
    {
        this.editUserService = editUserService;
    }
    [HttpGet]
    [Route("{id}")]
    public IActionResult Profile(string id)
    {
        ViewBag.Id = id;
        return View();
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(UserEdit userEdit)
    {
        var user = await editUserService.EditUser(userEdit);
        return RedirectToAction("Profile", new { id = user.Id });
    }

    public async Task<IActionResult> Delete(UserEdit userEdit)
    {
        await editUserService.DeleteUser(userEdit.Id);
        return RedirectToAction("", "Home");
    }
}