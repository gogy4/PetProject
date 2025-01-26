namespace PetProject.Models;

public class UserListModel
{
    public IEnumerable<User> Users { get; set; } = null;
    public string Name { get; set; }
}