using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class UserData
{
    [Required(ErrorMessage = "Не указано имя")]
    public string? Name { get; set; }
    [EmailAddress (ErrorMessage = "Не указан электронный адрес")]
    public string Email { get; set; } 
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}