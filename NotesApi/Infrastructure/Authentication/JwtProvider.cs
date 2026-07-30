using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtProvider
{
    private readonly JwtOptions _jwtOptions;
    
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new Claim("userId", user.Id.ToString()),
            new Claim("username", user.Username)
        ];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}