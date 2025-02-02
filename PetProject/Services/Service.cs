using PetProject.Data;
using PetProject.Utils;

namespace PetProject.Services;

public abstract class Service(AppDbContext db)
{
    protected readonly AppDbContext db = db;
    protected readonly UtilsForService utils = new();
}
