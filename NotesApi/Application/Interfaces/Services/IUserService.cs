using Application.DTOs.Requests;

namespace Application.Interfaces.Services;

public interface IUserService
{
    public Task RegisterUser(RegisterRequest request);
    public Task<string> LoginUser(LoginRequest request);
}