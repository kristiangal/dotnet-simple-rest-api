using RestAPI.Models;

namespace RestAPI.Repositories;

public interface IAuthRepository
{
    Task<User> CreateUser(User user);
    Task<bool> UserExists(int userId);
    Task<User?> FindByEmail(string userEmail);
}