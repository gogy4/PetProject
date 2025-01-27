using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class TextPaste
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    [Required]
    public string Content { get; set; }

    public TextPaste(string id, DateTime date, string content)
    {
        Id = id;
        Date = date;
        Content = content;
    }
}