using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class UserEdit 
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } 
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    public UserEdit()
    {
        
    }

    public UserEdit(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
    }
}