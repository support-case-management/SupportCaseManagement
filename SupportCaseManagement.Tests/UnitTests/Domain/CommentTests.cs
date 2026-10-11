using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Domain;

public class CommentTests
{
    [Fact]
    public void Constructor_withEmptyText_ThrowsArgumentException()
    {
        //Förbered en tom text och en tid
        string text = "";
        DateTime createdAt = DateTime.UtcNow;

        //Kontrollera att en tom kommentar nekas

        Assert.Throws<ArgumentException>(() => 
            new Comment(text, createdAt));
    }


[Fact]

    public void Constructor_withValidText_SetsPropertiesCorrectly()
    {
        // Arrange
        string text = "Skrivaren fungerar igen";
        DateTime createdAt = new(2026, 10, 11, 12, 0, 0, DateTimeKind.Utc);
        // Act
        Comment comment = new(text, createdAt);
        // Assert
        Assert.Equal(text, comment.Text);
        Assert.Equal(createdAt, comment.CreatedAt);
    }
}