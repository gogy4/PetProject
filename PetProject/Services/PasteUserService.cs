using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class PasteUserService(AppDbContext db) : Service(db)
{
    public bool CheckPassword(string newPassword, int confirmPassword)
    {
        return utils.CheckPassword(newPassword, confirmPassword);
    }

    public async Task EditUser(UserEdit userEdit)
    {
        var user = await db.Users.FindAsync(userEdit.Id);
        if (!string.IsNullOrEmpty(userEdit.Name)) user.Name = userEdit.Name;
        if (!string.IsNullOrEmpty(userEdit.Email)) user.Email = userEdit.Email;
        if (utils.CheckPassword(userEdit.Password, user.Password))
            user.Password = utils.GetHashedPassword(userEdit.NewPassword);
        db.Users.Update(user);
        await db.SaveChangesAsync();
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
        return await utils.GetUser(db, id: id);
    }

    public List<Paste> GetPastesByUserId(string userId)
    {
        return db.Pastes.Where(x => x.UserId == userId).ToList();
    }

    public async Task DeleteAllPastes(string userId)
    {
        var pastes = GetPastesByUserId(userId);
        db.Pastes.RemoveRange(pastes);
        await db.SaveChangesAsync();
    }
    
    public async Task DeletePaste(string id)
    {
        var paste = await db.Pastes.FirstOrDefaultAsync(p => p.Id == id);
        db.Pastes.Remove(paste);
        await db.SaveChangesAsync();
    }
}