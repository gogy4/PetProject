using System.ComponentModel.DataAnnotations;

namespace PetProject.Models;

public class TextPaste
{
    public string Id { get; init; }
    public DateTime Date { get; init; }
    [Required] public string Content { get; init; }

    public string UserId { get; init; }


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