using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class Paste
{
    [Key] public string Id { get; init; }
    public DateTime Date { get; init; }
    public DateTime ExpirationDate { get; init; }

    [Required] public byte[] Content { get; set; }
    public string UserId { get; init; }
}