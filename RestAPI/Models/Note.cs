using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models;

public class Note
{
    public long Id { get; set; }
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string Content { get; set; }
    public DateTime Created { get; set; }

    public int UserId { get; set; }
}