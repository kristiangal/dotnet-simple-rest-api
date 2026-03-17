using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Repositories;

public class NoteRepository:IRepository<Note>
{
    private readonly NoteContextPG _context;

    public NoteRepository(NoteContextPG context)
    {
        _context = context;
    }

    public async Task<Note?> GetById(long id)
    {
        return await _context.Notes.FindAsync(id);
    }

    public async Task<List<Note>> GetAll()
    {
        return await _context.Notes.ToListAsync();
    }

    public async Task<Note> Add(Note item)
    {
        var savedNote = await _context.Notes.AddAsync(item);
        return savedNote.Entity;
    }

    public Task<Note> Update(Note note)
    {
        _context.Notes.Update(note);
        return Task.FromResult(note);
    }

    public Task Delete(Note note)
    {
        _context.Notes.Remove(note);
        return Task.CompletedTask;
    }

    public async Task<List<Note>> Find(Expression<Func<Note, bool>> predicate)
    {
        return await _context.Notes.Where(predicate).ToListAsync();
    }
}