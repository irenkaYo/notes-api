using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NotesApiDbContext _context;

    public UserRepository(NotesApiDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        User? user = await _context.Users.FindAsync(username);
        return user;
    }
}