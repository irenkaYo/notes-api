namespace Domain.Models;

public class RefreshToken
{
    public string Token { get; set; } = null!;
    public Guid JwtId { get; set; } 
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Invalidated { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}