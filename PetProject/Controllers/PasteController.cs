using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers;

public class PasteController : Controller
{
    private readonly PasteService pasteService;

    public PasteController(PasteService pasteService)
    {
        this.pasteService = pasteService;
    }

    [HttpPost]
    [Route("Paste")]
    public async Task<IActionResult> Paste(string content, int expirationDays = 7) 
    {
        if (string.IsNullOrEmpty(content)) 
            return BadRequest("Content cannot be null or empty.");
    
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
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(string id)
    {
        var paste = pasteService.GetPasteById(id);

        if (paste == null)
        {
            TempData["Message"] = "Прошлая паста была удалена или не найдена";
            return RedirectToAction("Index", "Home");
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != paste.UserId)
        {
            TempData["Message"] = "Вы не можете удалить чужую пасту";
            return RedirectToAction("Index", "Home");
        }


        pasteService.DeletePaste(paste);
        TempData["Message"] = "Вы удалили прошлую пасту";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> EditPaste(string id, string content)
    {
        var paste = pasteService.GetPasteById(id);

        if (paste == null)
        {
            TempData["Message"] = "Прошлая паста была удалена или не найдена";
            return RedirectToAction("Index", "Home");
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != paste.UserId)
        {
            TempData["Message"] = "Вы не можете изменять чужую пасту";
            return RedirectToAction("Index", "Home");
        }
        var textPaste = await pasteService.EditPaste(paste, content);
        
        return PartialView(textPaste);
        
    }
}