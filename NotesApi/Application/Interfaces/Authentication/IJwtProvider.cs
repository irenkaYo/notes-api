using System.Security.Claims;
using Domain.Models;

namespace Application.Interfaces.Authentication;

public interface IJwtProvider
{
    public string GenerateToken(User user);
    public RefreshToken GenerateRefreshToken(Guid userId);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}