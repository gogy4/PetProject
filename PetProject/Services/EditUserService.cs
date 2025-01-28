using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class EditUserService
{
    private bool isCalculated;
    private int Hash;
    private readonly AppDbContext db;

    public EditUserService(AppDbContext db)
    {
        this.db = db;
    }
    
    public async Task<User> EditUser(UserEdit userEdit)
    {
       var user = await db.Users.FindAsync(userEdit.Id);
       if (!string.IsNullOrEmpty(userEdit.Name)) user.Name = userEdit.Name;
       if (!string.IsNullOrEmpty(userEdit.Email)) user.Email = userEdit.Email;
       if (!string.IsNullOrEmpty(userEdit.NewPassword))
       {
           if (CheckPassword(GetHashedPassword(userEdit.Password), user.Password))
               user.Password = GetHashedPassword(userEdit.NewPassword);
       }
       
       db.Users.Update(user);
       await db.SaveChangesAsync();
       return user;
    }

    private bool CheckPassword(int newPassword, int password)
    {
        return newPassword == password;
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
    
    public async Task DeleteUser(string id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return;
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    public async Task<UserEdit> GetUser(string id)
    {
        var user = await db.Users.FirstOrDefaultAsync(x=>x.Id == id);
        if (user is null) return null;
        return new UserEdit(user);
    }
}