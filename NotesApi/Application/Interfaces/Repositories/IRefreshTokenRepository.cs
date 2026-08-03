using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByToken(string token);
    public Task AddToken(RefreshToken refreshToken);
    public Task DeleteToken(RefreshToken refreshToken);
}