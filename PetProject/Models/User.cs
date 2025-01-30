using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetProject.Models;

public class User
{
    [HiddenInput]
    [BindNever]

    public string? Id { get; set; }
    [Required(ErrorMessage = "Не указано имя")]
    public string? Name { get; set; }
    [EmailAddress (ErrorMessage = "Не указан электронный адрес")]
    public string Email { get; set; } 
    [DataType(DataType.Password)]
    public int Password { get; set; }
}