using PetProject.Models;

namespace PetProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Paste> Pastes { get; set; }

    public async Task RemoveExpiredPastesAsync()
    {
        var expirationDate = DateTime.UtcNow.AddDays(-7);
        var expiredRecord = Pastes.Where(x=>x.Date < expirationDate);
        Pastes.RemoveRange(expiredRecord);
        await SaveChangesAsync();
    }
}