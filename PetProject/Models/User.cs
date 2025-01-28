using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class User
{
    public string Id { get; set; }
    [Required(ErrorMessage = "Не указано имя")]
    public string? Name { get; set; }
    [EmailAddress (ErrorMessage = "Не указан электронный адрес")]
    public string Email { get; set; } 
    [DataType(DataType.Password)]
    public int Password { get; set; }
}