using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Paste(string content)
    {
        if (string.IsNullOrEmpty(content)) return BadRequest("Content cannot be null or empty.");

        var pasteId = await pasteService.CreatePasteAsync(content);
        ViewData["PasteLink"] = Url.Action("GetPaste", "Home", new { id = pasteId.Id }, Request.Scheme);
        return RedirectToAction("GetPaste", new { id = pasteId.Id });
    }


    [HttpGet]
    [Route("paste/{id}")]
    public async Task<IActionResult> GetPaste(string id)
    {
        var paste = await pasteService.GetPasteWithTextByIdAsync(id);
        if (paste != null) return View(paste);
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

        pasteService.DeletePaste(paste);
        TempData["Message"] = "Вы удалили прошлую пасту";
        return RedirectToAction("Index", "Home");
    }
}