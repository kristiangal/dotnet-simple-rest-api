using System.Data;
using Microsoft.EntityFrameworkCore;
using RestAPI.DTO;
using RestAPI.Models;
using RestAPI.Repositories;

namespace RestAPI.Services;

public class NoteService : INoteService
{
    private readonly IRepository<Note> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NoteService> _logger;
    
    public NoteService(IRepository<Note> repository, IUnitOfWork unitOfWork, ILogger<NoteService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<NoteResponseDto>> GetAllNotes(int loggedInUserId)
    {
        var allNotes = await _repository.Find(note => note.UserId == loggedInUserId);
        return allNotes.Select(NoteToResponseDto);
    }

    public async Task<NoteResponseDto?> GetNoteById(long noteId, int loggedInUserId)
    {
        var foundNote = await _repository.Find(note => note.Id == noteId && note.UserId == loggedInUserId);
        var noteToReturn = foundNote.FirstOrDefault();
        return noteToReturn == null ? null : NoteToResponseDto(noteToReturn);
    }

    public async Task<NoteResponseDto> CreateNote(CreateNoteDto dto, int loggedInUserId)
    {
        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            Created = DateTime.UtcNow,
            UserId = loggedInUserId
        };
        
        await _repository.Add(note);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Note with id: {note.Id} was created for userId: {loggedInUserId}");
        
        return NoteToResponseDto(note);
    }

    public async Task<NoteResponseDto?> UpdateNote(long noteId, UpdateNoteDto dto, int loggedInUserId)
    {
        var findResult = await _repository.Find(note => note.Id == noteId &&
                                                      note.UserId == loggedInUserId);
        var noteToUpdate = findResult.FirstOrDefault(); 
        
        if (noteToUpdate == null) return null;

        noteToUpdate.Title = dto.Title;
        noteToUpdate.Content = dto.Content;

        var updatedNote = await _repository.Update(noteToUpdate);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Note with id: {updatedNote.Id} was updated for userId: {loggedInUserId}");
        
        return NoteToResponseDto(updatedNote);
    }

    public async Task<bool> DeleteNote(long noteId, int loggedInUserId)
    {
        var findResult = await _repository.Find(note => note.Id == noteId &&
                                                          note.UserId == loggedInUserId);
        var noteToRemove = findResult.FirstOrDefault();
        
        if (noteToRemove == null) return false;

        await _repository.Delete(noteToRemove);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Note with id: {noteToRemove.Id} was deleted for userId: {loggedInUserId}");
        
        return true;
    }

    public async Task<bool> NoteExists(long noteId)
    {
        return await _repository.GetById(noteId) != null;
    }

    private NoteResponseDto NoteToResponseDto(Note note)
    {
        return new NoteResponseDto(note.Id, note.Title, note.Content, note.Created, note.UserId);
    }
    
    
}