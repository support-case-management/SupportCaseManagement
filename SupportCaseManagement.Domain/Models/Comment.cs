namespace SupportCaseManagement.Domain.Models;

public class Comment
{
    public string Text { get; set; } 
    public DateTime CreatedAt { get; set; }


    public Comment(string text, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("kommentaren får inte vara tom");
        }

        Text = text;
        CreatedAt = createdAt;
    }
}
