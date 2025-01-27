using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PetProject.ModelsAttribute;

public class PasswordAttribute : ValidationAttribute
{
    private string message;
    public PasswordAttribute()
    {
    }

    public override bool IsValid(object value)
    {
        var password = value as string;

        if (password.Length < 8)
        {
            ErrorMessage = "Пароль должен быть длинее 7 символов";
            return false;
        }

        if (!IsMatch(password))
        {
            ErrorMessage = "Пароль должен содержать символы разного регистра и иметь специальные символы !@#$%^&*?";
            return false;
        }
        
        return true;
    }

    private bool IsMatch(string password)
    {
        return Regex.IsMatch(password, @"\d") || Regex.IsMatch(password, @"[!@#$%^&*?]") ||
               password.ToLower() != password || password != password.ToUpper();
    }
}