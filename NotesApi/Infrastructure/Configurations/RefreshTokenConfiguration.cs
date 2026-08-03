using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.HasKey(e => e.Token);
        
        builder.Property(e => e.JwtId)
            .IsRequired();
        
        builder.Property(e => e.ExpiryDate)
            .IsRequired();
        
        builder.Property(e => e.Invalidated)
            .IsRequired();
        
        builder.Property(e => e.UserId)
            .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        
        builder.Property(e => e.UpdatedAt);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId);
    }
}