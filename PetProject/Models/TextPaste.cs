using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class TextPaste
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    [Required]
    public string Content { get; set; }
    
    public string UserId { get; set; }


    public TextPaste(string id, DateTime date, string content, string userId)
    {
        Id = id;
        Date = date;
        Content = content;
        UserId = userId;
    }

    public TextPaste(Paste paste)
    {
        Id = paste.Id;
        Date = paste.Date;
        UserId = paste.UserId;
    }

    public TextPaste()
    {
        
    }
}