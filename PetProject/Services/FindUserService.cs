using PetProject.Data;

namespace PetProject.Utils;

public class FindUserService(AppDbContext db)
{
    public bool UserIsFound(string id)
    {
        return db.Users.Any(x => x.Id == id);
    }
}