using System.IO.Compression;
using System.Text;
using PetProject.Data;
using PetProject.Models;
using PetProject.Utils;

namespace PetProject.Services;

public class PasteService
{
    private readonly AppDbContext db;
    private readonly IdGenerator idGenerator = new();

    public PasteService(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<Paste> CreatePasteAsync(string content, string userId, int expirationDays)
    {
        var uniqueId = idGenerator.GenerateUniqueId();
        while (db.Pastes.Any(x => x.Id == uniqueId)) uniqueId = idGenerator.GenerateUniqueId();

        if (userId is null) userId = "Не авторизован";

        var paste = new Paste
        {
            Id = uniqueId,
            Date = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(expirationDays), // Устанавливаем срок действия
            Content = CompressString(content),
            UserId = userId
        };
        db.Pastes.Add(paste);
        await db.SaveChangesAsync();

        return paste;
    }



    public async Task<TextPaste?> GetPasteWithText(string id)
    {
        var paste = await db.Pastes.FirstOrDefaultAsync(x => x.Id == id && x.ExpirationDate > DateTime.UtcNow);
        if (paste is null) return null;
        return new TextPaste(paste.Id, paste.Date, DecompressString(paste.Content), paste.UserId);
    }



    public Paste GetPasteById(string id)
    {
        return db.Pastes.FirstOrDefault(p => p.Id == id);
    }

    private byte[] CompressString(string content)
    {
        var byteArr = new byte[0];
        if (!string.IsNullOrEmpty(content))
        {
            byteArr = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream())
            {
                using (var zip = new GZipStream(stream, CompressionMode.Compress))
                {
                    zip.Write(byteArr, 0, byteArr.Length);
                }

                byteArr = stream.ToArray();
            }
        }

        return byteArr;
    }

    private string DecompressString(byte[] byteArr)
    {
        var resultString = string.Empty;
        if (byteArr == null || byteArr.Length <= 0) return resultString;
        using var stream = new MemoryStream(byteArr);
        using var zip = new GZipStream(stream, CompressionMode.Decompress);
        using var reader = new StreamReader(zip);
        resultString = reader.ReadToEnd();

        return resultString;
    }

    public async Task DeletePaste(Paste paste)
    {
        db.Pastes.Remove(paste);
        await db.SaveChangesAsync();
    }

    public async Task<TextPaste> EditPaste(Paste paste, string content)
    {
        paste.Content = CompressString(content);
        db.Pastes.Update(paste);
        await db.SaveChangesAsync();
        var textPaste = new TextPaste(paste)
        {
            Content = content
        };
        return textPaste;
    }
}