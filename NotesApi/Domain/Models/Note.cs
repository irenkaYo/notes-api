namespace Domain.Models;

public class Note
{
    public Guid Id { get; private set; }
    public string Title { get; set; }
    public string? Text { get;  set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public Note(string title, string? text, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Text = text;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
    }
}