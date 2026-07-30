namespace Application.DTOs.Responses;

public class NoteResponse
{
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public NoteResponse(string title, string text, DateTime createdAt)
    {
        Title = title;
        Text = text;
        CreatedAt = createdAt;
    }
}