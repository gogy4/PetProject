using PetProject.Models;

namespace PetProject.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Paste> Pastes { get; set; }

    public async Task RemoveExpiredPastesAsync()
    {
        var expiredPastes = await Pastes
            .Where(p => p.ExpirationDate <= DateTime.UtcNow)
            .ToListAsync();

        if (expiredPastes.Any())
        {
            Pastes.RemoveRange(expiredPastes);
            await SaveChangesAsync();
        }
    }
}