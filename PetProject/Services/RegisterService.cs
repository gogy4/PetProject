using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class RegisterService(AppDbContext db) : Service(db)
{
    public async Task<User> SaveHashedPassword(RegisterViewModel userData)
    {
        var uniqueId = utils.GenerateUniqueId();
        while (db.Pastes.Any(x => x.Id == uniqueId)) uniqueId = utils.GenerateUniqueId();
        var passwordHash = utils.GetHashedPassword(userData.Password);
        var user = new User
        {
            Id = uniqueId,
            Email = userData.Email,
            Name = userData.Name,
            Password = passwordHash
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }
    
    public async Task<bool> CheckCriteriaPassword(RegisterViewModel user, string password, ModelStateDictionary modelState)
    {
        return await utils.CheckCriteriaPassword(user, password, modelState,db);
    }
}