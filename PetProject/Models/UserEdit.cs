using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetProject.Models;

public class UserEdit 
{
    [HiddenInput]
    [BindNever]
    public string? Id { get; set; }
    [Required(ErrorMessage = "Имя не может быть пустым")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string? Name { get; set; }
    [EmailAddress]
    [Required(ErrorMessage = "Почта не может быть пустой")]
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