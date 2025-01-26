using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers
{
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
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Content cannot be null or empty.");
            }

            var pasteId = await pasteService.CreatePasteAsync(content);
            ViewData["PasteLink"] = Url.Action("GetPaste", "Home", new { id = pasteId.Id }, Request.Scheme);
            return RedirectToAction("GetPaste", new { id = pasteId.Id });
        }


        [HttpGet]
        [Route("paste/{id}")]
        public async Task<IActionResult> GetPaste(string id)
        {
            var paste = await pasteService.GetPasteByIdAsync(id);
            if (paste == null) return NotFound("Paste not found");

            return View(paste);
        }
    }

}