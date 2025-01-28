using PetProject.Data;
using PetProject.Models;
using PetProject.Utils;

namespace PetProject.Services;

public class RegisterService
{
    private bool isCalculated;
    private int Hash;
    private readonly AppDbContext db;
    private IdGenerator idGenerator = new();

    public RegisterService(AppDbContext db)
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

    public async Task<User> SaveHashedPassword(UserData userData)
    {
        var uniqueId = idGenerator.GenerateUniqueId();
        while (db.Pastes.Any(x => x.Id == uniqueId))
        {
            uniqueId = idGenerator.GenerateUniqueId();
        }
        var passwordHash = GetHashedPassword(userData.Password);
        var user = new User
        {
            Id = uniqueId,
            Email = userData.Email,
            Name = userData.Name,
            Password = passwordHash,
            IsRegistrationComplete = true
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }
}