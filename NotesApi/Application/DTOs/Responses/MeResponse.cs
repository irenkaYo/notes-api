namespace Application.DTOs.Responses;

public class MeResponse
{
    public string? UserId { get; set; }
    public string? Username { get; set; }

    public MeResponse(string? userId, string? username)
    {
        UserId = userId;
        Username = username;
    }
}