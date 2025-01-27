using PetProject.Data;
using PetProject.Models;

namespace PetProject.Services;

public class RegisterService
{
    private readonly AppDbContext db;

    public RegisterService(AppDbContext db)
    {
        this.db = db;
    }
}