using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public interface IUser
{
    public string? Id { get; }

    [Required(ErrorMessage = "Имя не может быть пустым")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string? Name { get; }

    [EmailAddress]
    [Required(ErrorMessage = "Почта не может быть пустой")]
    public string Email { get; }

    [DataType(DataType.Password)] public string? Password { get; }
}