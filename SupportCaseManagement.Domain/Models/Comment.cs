namespace SupportCaseManagement.Domain.Models;

public class Comment
{
    public string Text { get; set; } 
    public DateTime Created { get; set; }


    public Comment(string text, DateTime created)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("kommentaren får inte vara tom");
        }

        Text = text;
        Created = created;
    }
}
