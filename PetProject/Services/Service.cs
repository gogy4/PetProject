using PetProject.Data;
using PetProject.Utils;

namespace PetProject.Services;

public abstract class Service
{
    protected readonly AppDbContext db;
    protected readonly UtilsForService utils = new();

    // Конструктор для инициализации db
    public Service(AppDbContext db)
    {
        this.db = db;
    }
}
