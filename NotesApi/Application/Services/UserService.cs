using System.Security.Claims;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task RegisterUser(RegisterRequest request)
    {
        if (request.Username.Length < 3 || request.Username.Length > 20)
            throw new ArgumentException("Username must be between 3 and 20 characters long.");
        
        if (request.Password.Length < 5)
            throw new ArgumentException("Password must be at least 5 characters long.");
        
        var existing = await _userRepository.GetUserByUsername(request.Username);
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");
        
        string passwordHashed = _passwordHasher.Generate(request.Password);
        User user = new User(request.Username, passwordHashed);
        await _userRepository.AddUser(user);
    }

    public async Task<string> LoginUser(LoginRequest request)
    {
        User? user = await _userRepository.GetUserByUsername(request.Username);
        if (user is null)
            throw new ArgumentException("Username does not exist.");
        
        bool result = _passwordHasher.Verify(request.Password, user.PasswordHashed);
        if (!result)
            throw new ArgumentException("Password does not match.");
        
        string token = _jwtProvider.GenerateToken(user);
        return token;
    }

    public async Task<RefreshTokenResponse> CreateRefreshToken(RefreshTokenRequest request)
    {
        var existingToken = await _refreshTokenRepository.GetByToken(request.RefreshToken);

        if (existingToken is null)
            throw new ArgumentException("Refresh token not found.");

        if (existingToken.ExpiryDate < DateTime.UtcNow)
        {
            await _refreshTokenRepository.DeleteToken(existingToken);
            throw new ArgumentException("Refresh token expired.");
        }
        
        var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.Token);
        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        if (existingToken.UserId != userId)
            throw new ArgumentException("Refresh token does not match.");

        var user = await _userRepository.GetUserById(userId);

        if (user is null)
            throw new ArgumentException("User not found");
        
        string accessToken = _jwtProvider.GenerateToken(user);
        
        var newRefreshToken = _jwtProvider.GenerateRefreshToken(userId);
        await _refreshTokenRepository.DeleteToken(existingToken);
        await _refreshTokenRepository.AddToken(newRefreshToken);

        var result = new RefreshTokenResponse(accessToken, newRefreshToken.Token);
        return result;
    }
}