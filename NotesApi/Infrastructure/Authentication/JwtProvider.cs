using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public JwtProvider(IOptions<JwtOptions> options, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtOptions = options.Value;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        ];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresHours));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<string> GenerateRefreshToken(string token, User user, string? existingRefreshToken)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        if (!string.IsNullOrEmpty(existingRefreshToken))
        {
            var existingToken = await _refreshTokenRepository.GetByToken(existingRefreshToken);

            if (existingToken != null)
            {
                await _refreshTokenRepository.DeleteToken(existingToken);
            }
        }

        await _refreshTokenRepository.AddToken(refreshToken);

        return refreshToken.Token;
    }
}