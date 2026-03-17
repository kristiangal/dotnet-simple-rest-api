using RestAPI.DTO;
using RestAPI.Models;

namespace RestAPI.Services;

public interface INoteService
{
    Task<NoteResponseDto?> GetNoteById(long noteId, int loggedInUserId);
    Task<IEnumerable<NoteResponseDto>> GetAllNotes(int loggedInUserId);
    Task<NoteResponseDto> CreateNote(CreateNoteDto dto, int loggedInUserId);
    Task<NoteResponseDto?> UpdateNote(long noteId, UpdateNoteDto dto, int loggedInUserId);
    Task<bool> DeleteNote(long noteId, int loggedInUserId);
    Task<bool> NoteExists(long noteId);
}