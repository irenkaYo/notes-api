using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface IUserService
{
    public Task RegisterUser(RegisterRequest request);
    public Task<RefreshTokenResponse> LoginUser(LoginRequest request);
    public Task<RefreshTokenResponse> UpdateRefreshToken(RefreshTokenRequest request);
}