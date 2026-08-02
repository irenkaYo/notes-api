namespace Application.DTOs.Responses;

public class MeResponse
{
    public string? Id { get; set; }
    public string? Username { get; set; }

    public MeResponse(string? id, string? username)
    {
        Id = id;
        Username = username;
    }
}