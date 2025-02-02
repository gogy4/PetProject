using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class LogInService(AppDbContext db) : Service(db)
{
    public Task<bool> CheckPassword(User user, string password)
    {
        return utils.CheckCriteriaPassword(user, password);
    }

    public async Task<User> GetUser(string email)
    {
        return await utils.GetUser(db, email);
    }
}