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

    public bool CheckCriteriaPassword(string newPassword, int password)
    {
        return !string.IsNullOrEmpty(newPassword) && GetHashedPassword(newPassword) == password;
    }

    public async Task<bool> CheckCriteriaPassword(User user, string password)
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

    public async Task<bool> CheckEmail(string email, AppDbContext db, string id = null)
    {
        return await db.Users.AnyAsync(x => x.Id != id && x.Email == email);
    }

    public async Task<bool> CheckName(string name)
    {
        return name.Length > 1 && name.Length < 51;
    }

    public async Task<bool> CheckCriteriaPassword(IUser user, string password, ModelStateDictionary modelsState,
        AppDbContext db, string operation = null)
    {
        var error = false;
        if ((operation == "ChangeName" || operation is null) && !await CheckName(user.Name))
        {
            modelsState.AddModelError("Name", "Имя должно быть от 2 до 50 символов");
            error = true;
        }

        if ((operation == "ChangeEmail" || operation is null) && await CheckEmail(user.Email, db, user.Id))
        {
            modelsState.AddModelError("Email", "Данная почта уже зарегистрирована");
            error = true;
        }

        if (operation == "ChangePassword" || operation is null)
        {
            if (password.Length < 8)
            {
                modelsState.AddModelError("Password", "Пароль должен быть длиннее 7 символов");
                error = true;
            }

            if (password.ToLower() == password ||
                password.ToUpper() == password)
            {
                modelsState.AddModelError("Password", "Символы в пароле должны быть разного регистра," +
                                                      " хотя бы один символ должен отличаться по регистру от других");
                error = true;
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*?_\-+=]"))
            {
                modelsState.AddModelError("Password", "Пароль должен содержать хотя бы" +
                                                      " один из специальных символов !@#$%^&*?_\\-+=");
                error = true;
            }

            if (operation == "ChangePassword")
            {
                var userForCheckPassword = await GetUser(db, id: user.Id);
                var passwordCorrect = CheckCriteriaPassword(user.Password, userForCheckPassword.Password);
                if (!passwordCorrect)
                {
                    modelsState.AddModelError("Password", "Вы ввели неверный прошлый пароль");
                    error = true;
                }
            }
        }


        return error;
    }
}