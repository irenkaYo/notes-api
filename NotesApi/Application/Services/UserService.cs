using Application.DTOs.Requests;
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
    
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task RegisterUser(RegisterRequest request)
    {
        if (request.Username.Length < 3 || request.Username.Length > 20)
            throw new ArgumentException("Username must be between 3 and 20 characters long.");
        
        if (request.Password.Length < 5)
            throw new ArgumentException("Password must be at least 5 characters long.");
        
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
}