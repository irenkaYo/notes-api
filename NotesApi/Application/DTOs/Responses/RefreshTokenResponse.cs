namespace Application.DTOs.Responses;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public RefreshTokenResponse(string token, string refreshToken)
    {
        AccessToken = token;
        RefreshToken = refreshToken;
    }
}