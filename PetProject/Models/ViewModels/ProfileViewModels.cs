namespace PetProject.Models;

public class ProfileViewModel
{
    public UserEdit User{ get; set; }
    public List<Paste> Pastes { get; set; } = new();
}