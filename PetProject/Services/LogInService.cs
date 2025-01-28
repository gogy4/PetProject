using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class LogInService
{
    private AppDbContext db;
    private bool isCalculated;
    private int Hash;

    public LogInService(AppDbContext db)
    {
        this.db = db;
    }

    private int GetHashedPassword(string password)
    {
        const int hash = 499;
        unchecked
        {
            if (isCalculated) return Hash;

            for (var i = 0; i < password.Length - 1; i++)
                Hash +=
                    (int)(password[i] * Math.Pow(hash, password.Length - 1 - i) + password[i + 1]);

            isCalculated = true;
            return Hash;
        }
    }

    public Task<bool> CheckCorrectPassword(User user, string password)
    {
        var hashedPassword = GetHashedPassword(password);
        return Task.FromResult(user.Password == hashedPassword);
    }

    public async Task<User> GetUser(string email)
    {
        return await db.Users.FirstOrDefaultAsync(x=>x.Email == email);
    }
}