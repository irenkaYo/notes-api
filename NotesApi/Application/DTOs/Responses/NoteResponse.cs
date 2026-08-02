namespace Application.DTOs.Responses;

public class NoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public NoteResponse(Guid id, string title, string text, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Text = text;
        CreatedAt = createdAt;
    }
}