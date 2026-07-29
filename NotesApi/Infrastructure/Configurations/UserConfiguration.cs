using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Username)
            .IsRequired();

        builder.Property(x => x.PasswordHashed)
            .IsRequired();

        builder.HasIndex(x => x.Username)
            .IsUnique();
    }
}