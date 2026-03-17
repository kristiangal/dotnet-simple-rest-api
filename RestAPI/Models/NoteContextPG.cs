using Microsoft.EntityFrameworkCore;

namespace RestAPI.Models;

public class NoteContextPG(DbContextOptions<NoteContextPG> options): DbContext(options)
{
    public DbSet<Note> Notes  { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>().ToTable("notes");
        
        modelBuilder.Entity<User>()
                    .ToTable("users")
                    .HasIndex(u => u.Email)
                    .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}