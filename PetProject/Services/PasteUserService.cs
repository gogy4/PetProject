using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class PasteUserService
{
    private readonly AppDbContext db;

    public PasteUserService(AppDbContext db)
    {
        this.db = db;
    }
    
    public async Task EditUser(UserEdit userEdit)
    {
       var user = await db.Users.FindAsync(userEdit.Id);
       if (!string.IsNullOrEmpty(userEdit.Name)) user.Name = userEdit.Name;
       if (!string.IsNullOrEmpty(userEdit.Email)) user.Email = userEdit.Email;
       if (CheckPassword(userEdit.Password, user.Password))
           user.Password = GetHashedPassword(userEdit.NewPassword);
       db.Users.Update(user);
       await db.SaveChangesAsync();
    }

    public bool CheckPassword(string newPassword, int password)
    {
        return !string.IsNullOrEmpty(newPassword) && GetHashedPassword(newPassword) == password;
    }
    
    private int GetHashedPassword(string password)
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
    
    public async Task DeleteUser(string id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return;
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    public async Task<User> GetUser(string id)
    {
        var user = await db.Users.FirstOrDefaultAsync(x=>x.Id == id);
        return user;
    }
}