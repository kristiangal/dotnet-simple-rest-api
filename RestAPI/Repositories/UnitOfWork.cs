using RestAPI.Models;

namespace RestAPI.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly NoteContextPG _context;

    public UnitOfWork(NoteContextPG context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}