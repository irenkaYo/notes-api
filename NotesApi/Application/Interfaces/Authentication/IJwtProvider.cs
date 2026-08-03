using Domain.Models;

namespace Application.Interfaces.Authentication;

public interface IJwtProvider
{
    public string GenerateToken(User user);
    public Task<string> GenerateRefreshToken(string token, User user, string? existingRefreshToken);
}