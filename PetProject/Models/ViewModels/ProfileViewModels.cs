namespace PetProject.Models;

public class ProfileViewModel
{
    public ProfileUserEditViewModel User{ get; set; }
    public List<Paste> Pastes { get; set; } = new();
}