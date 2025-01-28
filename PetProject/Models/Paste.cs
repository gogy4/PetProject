using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class Paste
{
    [Key]
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime ExpirationDate { get; set; }

    [Required]
    public Byte[] Content { get; set; }
    public string UserId { get; set; }
}