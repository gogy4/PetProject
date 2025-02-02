using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class RegisterViewModel : IUser
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Имя не может быть пустым")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string? Name { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Почта не может быть пустой")]
    public string Email { get; set; }

    [DataType(DataType.Password)] public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}