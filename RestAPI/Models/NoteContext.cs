using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Models;

public class NoteContext: DbContext
{
    public NoteContext(DbContextOptions<NoteContext> options) : base(options)
    {
    }

    public List<Note> Notes { get; set; } = null!;

public DbSet<RestAPI.Models.Note> Note { get; set; } = default!;
}