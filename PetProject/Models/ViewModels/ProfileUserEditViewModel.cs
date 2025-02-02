using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class ProfileUserEditViewModel : IUser
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Имя не может быть пустым")]
    public string? Name { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Почта не может быть пустой")]
    public string Email { get; set; }

    [DataType(DataType.Password)] public string? Password { get; set; }

    [DataType(DataType.Password)] public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string? PasswordConfirm { get; set; } 

    public ProfileUserEditViewModel()
    {
    }

    public ProfileUserEditViewModel(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
    }
}