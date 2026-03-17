using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public List<Note> Notes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}