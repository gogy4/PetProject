using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PetProject.Services;

namespace PetProject.Controllers;

public class PasteController(PasteService pasteService) : Controller
{
    [HttpPost]
    [Route("Paste")]
    public async Task<IActionResult> Paste(string content, int expirationDays = 7)
    {
        if (string.IsNullOrEmpty(content))
        {
            TempData["Message"] = "Паста не может быть пустой";
            return RedirectToAction("index", "home"); 
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var paste = await pasteService.CreatePasteAsync(content, currentUserId, expirationDays);

        ViewData["PasteLink"] = Url.Action("GetPaste", "Paste", new { id = paste.Id }, Request.Scheme);
        return RedirectToAction("GetPaste", new { id = paste.Id });
    }


    [HttpGet]
    [Route("paste/{id}")]
    public async Task<IActionResult> GetPaste(string id, string userId)
    {
        var textPaste = await pasteService.GetPasteWithText(id);
        if (textPaste != null) return View(textPaste);
        TempData["Message"] = "Прошлая паста была удалена или не найдена";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ActionName("DeleteUser")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var paste = await pasteService.GetPasteById(id);

        var message = await pasteService.CheckRights(HttpContext, paste);
        if (message.Length > 0)
        {
            TempData["Message"] = message;
            return RedirectToAction("Index", "Home");
        }


        await pasteService.DeletePaste(paste);
        TempData["Message"] = "Вы удалили прошлую пасту";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> EditPaste(string id, string content)
    {
        var paste = await pasteService.GetPasteById(id);

        var message = await pasteService.CheckRights(HttpContext, paste);
        if (message.Length > 0)
        {
            TempData["Message"] = message;
            return RedirectToAction("Index", "Home");
        }

        var textPaste = await pasteService.EditPaste(paste, content);

        return PartialView(textPaste);
    }
}