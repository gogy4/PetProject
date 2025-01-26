using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class Paste
{
    [Key]
    public string Id { get; set; }
    public DateTime Date { get; set; }
    [Required]
    public string Content { get; set; }
}