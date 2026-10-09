namespace SupportCaseManagement.Domain.Models;

public class Comment
{
    public string Text { get; } 
    public DateTime CreatedAt { get; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string CreatedAtText => 
        CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

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
