namespace Domain.Models;

public class Note
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public Note(string title, string text, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Text = text;
        CreatedAt = DateTime.Now;
        UserId = userId;
    }
}