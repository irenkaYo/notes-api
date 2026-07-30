using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    public Task AddUser(User user);
    public Task<User?> GetUserByUsername(string username);
}