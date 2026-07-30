using Domain.Models;

namespace Application.Interfaces.Authentication;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}