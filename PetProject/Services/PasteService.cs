using System.Linq;
using System.Threading.Tasks;
using PetProject.Data;
using PetProject.Models;
using PetProject.Utils;

namespace PetProject.Services;

public class PasteService
{
    private readonly AppDbContext _context;

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
            uniqueId = IdGenerator.GenerateUniqueId();
        }
        while (_context.Pastes.Any(x => x.Id == uniqueId)); // Проверяем, что ID действительно уникальный

        var paste = new Paste
        {
            Id = uniqueId,
            Date = DateTime.UtcNow,
            Content = content
        };

        _context.Pastes.Add(paste);
        await _context.SaveChangesAsync();

        return paste;
    }

    
    public async Task<Paste?> GetPasteByIdAsync(string id)
    {
        return await _context.Pastes.FirstOrDefaultAsync(p => p.Id == id);
    }
}