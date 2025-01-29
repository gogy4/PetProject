using PetProject.Data;
using PetProject.Models;

namespace PetProject.Utils;

public class UtilsForService
{
    public int GetHashedPassword(string password)
    {
        const int hash = 499;
        var hashPassword = 0;
        unchecked
        {
            for (var i = 0; i < password.Length - 1; i++)
                hashPassword +=
                    (int)(password[i] * Math.Pow(hash, password.Length - 1 - i) + password[i + 1]);

            return hashPassword;
        }
    }

    public bool CheckPassword(string newPassword, int password)
    {
        return !string.IsNullOrEmpty(newPassword) && GetHashedPassword(newPassword) == password;
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        var hashedPassword = GetHashedPassword(password);
        return user.Password == hashedPassword;
    }

    public async Task<User> GetUser(AppDbContext db, string email = null, string id = null)
    {
        if (id is null) return db.Users.FirstOrDefault(x => x.Email == email);
        if (email is null)  return db.Users.FirstOrDefault(x => x.Id == id);
        throw new ArgumentException("Нельзя искать по пустым полям");
    }

    private string alphabet
    {
        get
        {
            return string.Concat(
                new string(Enumerable.Range('A', 26).Select(i => (char)i).ToArray()),
                new string(Enumerable.Range('a', 26).Select(i => (char)i).ToArray()),
                new string(Enumerable.Range('0', 10).Select(i => (char)i).ToArray()));
        }
    }

    private readonly Random random = new();

    public string GenerateUniqueId(int length = 8)
    {
        return new string(Enumerable.Repeat(alphabet, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}