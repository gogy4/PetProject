namespace PetProject.Utils;

public class IdGenerator
{
    private readonly Random _random = new();
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public string GenerateUniqueId(int length = 8)
    {
        return new string(Enumerable.Repeat(Alphabet, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}

