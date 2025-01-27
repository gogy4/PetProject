using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PetProject.Data;
using PetProject.Models;
using PetProject.Utils;

namespace PetProject.Services;

public class PasteService
{
    private readonly AppDbContext _context;
    private readonly IdGenerator idGenerator = new IdGenerator();

    public PasteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Paste> CreatePasteAsync(string content)
    {
        string uniqueId;

        // Генерация первого уникального ID до входа в цикл
        do
        {
            uniqueId = idGenerator.GenerateUniqueId();
        }
        while (_context.Pastes.Any(x => x.Id == uniqueId)); // Проверяем, что ID действительно уникальный

        var paste = new Paste
        {
            Id = uniqueId,
            Date = DateTime.UtcNow,
            Content = CompressString(content)
        };
        _context.Pastes.Add(paste);
        await _context.SaveChangesAsync();

        return paste;
    }

    
    public async Task<TextPaste?> GetPasteByIdAsync(string id)
    {
        var paste = await _context.Pastes.FirstOrDefaultAsync(p => p.Id == id);
        return new TextPaste(paste.Id, paste.Date, DecompressString(paste.Content));
    }

    public Byte[] CompressString(string content)
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

    public string DecompressString(Byte[] byteArr)
    {
        var resultString = string.Empty;
        if (byteArr != null && byteArr.Length > 0)
        {
            using (MemoryStream stream = new MemoryStream(byteArr))
            using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(zip))
            {
                resultString = reader.ReadToEnd(); 
            }
        }
        
        return resultString;
    }
    
}