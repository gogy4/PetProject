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

    public async Task<Paste> CreatePasteAsync(string content)
    {
        string uniqueId;

        // Генерация первого уникального ID до входа в цикл
        do
        {
            uniqueId = idGenerator.GenerateUniqueId();
        } while (db.Pastes.Any(x => x.Id == uniqueId)); // Проверяем, что ID действительно уникальный

        var paste = new Paste
        {
            Id = uniqueId,
            Date = DateTime.UtcNow,
            Content = CompressString(content)
        };
        db.Pastes.Add(paste);
        await db.SaveChangesAsync();

        return paste;
    }


    public async Task<TextPaste?> GetPasteWithTextByIdAsync(string id)
    {
        var paste = await db.Pastes.FirstOrDefaultAsync(p => p.Id == id);
        if (paste == null) return null; // Возвращаем null, если объект не найден

        return new TextPaste(paste.Id, paste.Date, DecompressString(paste.Content));
    }


    public Paste GetPasteById(string id)
    {
        var paste = db.Pastes.FirstOrDefault(p => p.Id == id);
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
        if (byteArr != null && byteArr.Length > 0)
            using (var stream = new MemoryStream(byteArr))
            using (var zip = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(zip))
            {
                resultString = reader.ReadToEnd();
            }

        return resultString;
    }

    public void DeletePaste(Paste paste)
    {
        db.Pastes.Remove(paste);
        db.SaveChanges();
    }
}