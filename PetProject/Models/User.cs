using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PetProject.ModelsAttribute;

namespace PetProject.Models;

public class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Не указано имя")]
    public string? Name { get; set; }
    [EmailAddress (ErrorMessage = "Не указан электронный адрес")]
    public string Email { get; set; } 
    [DataType(DataType.Password)]
    [Password]
    public string? Password { get; set; }
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}