using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetProject.Data;
using PetProject.Models;

namespace PetProject.Utils;

public class UtilsForService
{
    public int GetHashedPassword(string password)
    {
        const int hash = 499;
        var hashPassword = 0;
        unchecked
        {
            for (var i = 0; i < password.Length - 1; i++)
                hashPassword +=
                    (int)(password[i] * Math.Pow(hash, password.Length - 1 - i) + password[i + 1]);

            return hashPassword;
        }
    }

    public bool CheckPassword(string newPassword, int password)
    {
        return !string.IsNullOrEmpty(newPassword) && GetHashedPassword(newPassword) == password;
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        var hashedPassword = GetHashedPassword(password);
        return user.Password == hashedPassword;
    }

    public async Task<User> GetUser(AppDbContext db, string email = null, string id = null)
    {
        if (id is null) return db.Users.FirstOrDefault(x => x.Email == email);
        if (email is null) return db.Users.FirstOrDefault(x => x.Id == id);
        throw new ArgumentException("Нельзя искать по пустым полям");
    }

    private string alphabet
    {
        get
        {
            return string.Concat(
                new string(Enumerable.Range('A', 26).Select(i => (char)i).ToArray()),
                new string(Enumerable.Range('a', 26).Select(i => (char)i).ToArray()),
                new string(Enumerable.Range('0', 10).Select(i => (char)i).ToArray()));
        }
    }

    private readonly Random random = new();

    public string GenerateUniqueId(int length = 8)
    {
        return new string(Enumerable.Repeat(alphabet, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private async Task<bool> CheckName(string name)
    {
        return name.Length > 1 && name.Length < 51;
    }

    private async Task<bool> CheckEmail(string email, AppDbContext db, string id = null)
    {
        return !await db.Users.AnyAsync(x => x.Id != id && x.Email == email);
    }

    public async Task<bool> CheckCriteriaPassword(IUser user, string password, ModelStateDictionary modelsState,
        AppDbContext db, string operation = null)
    {
        var error = await CheckNameEmailCriteria(modelsState, await CheckName(user.Name), 
            "ChangeName", "Name", "Имя должно быть от 2 до 50 символов", operation);

        error = await CheckNameEmailCriteria(modelsState, await CheckEmail(user.Email, db, user.Id), 
            "ChangeEmail", "Email", "Данная почта уже зарегистрирована", operation);


        error = await CheckPasswordCriteria(user, modelsState, db, password, operation);


        return error;
    }


    private async Task<bool> CheckNameEmailCriteria(ModelStateDictionary modelsState, bool additionalValidation,
        string currentOperations, string keyError, string valueError,
        string operation = null)
    {
        if ((operation != currentOperations && operation is not null) || additionalValidation) return false;
        modelsState.AddModelError(keyError, valueError);
        return true;
    }

    private async Task<bool> CheckPasswordCriteria(IUser user, ModelStateDictionary modelState, AppDbContext db,
        string password, string operation)

    {
        var error = false;
        if (operation != "ChangePassword" && operation is not null) return error;
        if (password.Length < 8)
        {
            modelState.AddModelError("Password", "Пароль должен быть длиннее 7 символов");
            error = true;
        }

        if (password.ToLower() == password ||
            password.ToUpper() == password)
        {
            modelState.AddModelError("Password", "Символы в пароле должны быть разного регистра," +
                                                 " хотя бы один символ должен отличаться по регистру от других");
            error = true;
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*?_\-+=]"))
        {
            modelState.AddModelError("Password", "Пароль должен содержать хотя бы" +
                                                 " один из специальных символов !@#$%^&*?_\\-+=");
            error = true;
        }

        if (operation != "ChangePassword") return error;
        var userForCheckPassword = await GetUser(db, id: user.Id);
        var passwordCorrect = CheckPassword(user.Password, userForCheckPassword.Password);
        if (passwordCorrect) return error;
        modelState.AddModelError("Password", "Вы ввели неверный прошлый пароль");
        error = true;

        return error;
    }
}