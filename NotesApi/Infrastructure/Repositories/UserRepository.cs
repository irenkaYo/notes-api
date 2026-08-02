using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        return user;
    }
}