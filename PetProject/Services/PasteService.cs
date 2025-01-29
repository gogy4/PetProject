using PetProject.Data;
using PetProject.Models;
using PetProject.Utils;

namespace PetProject.Services;

public class PasteService(AppDbContext db) : Service(db)
{
    private readonly PasteUtils pasteUtils = new();


    public async Task<Paste> CreatePasteAsync(string content, string userId, int expirationDays)
    {
        var uniqueId = utils.GenerateUniqueId();

        await using var transaction = await db.Database.BeginTransactionAsync();
        while (await db.Pastes.AnyAsync(x => x.Id == uniqueId)) uniqueId = utils.GenerateUniqueId();

        if (userId is null) userId = "Не авторизован";

        var paste = new Paste
        {
            Id = uniqueId,
            Date = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(expirationDays),
            Content = pasteUtils.CompressString(content),
            UserId = userId
        };

        db.Pastes.Add(paste);
        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return paste;
    }


    public async Task<TextPaste?> GetPasteWithText(string id)
    {
        var paste = await db.Pastes.FirstOrDefaultAsync(x => x.Id == id && x.ExpirationDate > DateTime.UtcNow);
        if (paste is null) return null;
        return new TextPaste(paste.Id, paste.Date, pasteUtils.DecompressString(paste.Content), paste.UserId);
    }


    public Paste GetPasteById(string id)
    {
        return db.Pastes.FirstOrDefault(p => p.Id == id);
    }


    public async Task DeletePaste(Paste paste)
    {
        db.Pastes.Remove(paste);
        await db.SaveChangesAsync();
    }

    public async Task<TextPaste> EditPaste(Paste paste, string content)
    {
        paste.Content = pasteUtils.CompressString(content);
        db.Pastes.Update(paste);
        await db.SaveChangesAsync();
        var textPaste = new TextPaste(paste)
        {
            Content = content
        };
        return textPaste;
    }
}