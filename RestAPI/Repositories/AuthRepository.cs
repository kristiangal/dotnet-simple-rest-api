using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Repositories;

public class AuthRepository:IAuthRepository
{
    private readonly NoteContextPG _context;

    public AuthRepository(NoteContextPG context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task<bool> UserExists(int userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async  Task<User?> FindByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == userEmail);
    }
}