using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.DTO;
using RestAPI.Services;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _service;

        public NotesController(INoteService service)
        {
            _service = service;
        }

        // GET: api/Notes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NoteResponseDto>>> GetNotes()
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var notes = await _service.GetAllNotes(userId);
            return Ok(notes);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<NoteResponseDto>> GetNote(long id)
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var note = await _service.GetNoteById(id, userId);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<NoteResponseDto>> PutNote(long id, UpdateNoteDto note)
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var updatedNote =  await _service.UpdateNote(id, note, userId);
            
            if (updatedNote == null) return NotFound();

            return Ok(updatedNote);
        }

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<NoteResponseDto>> PostNote(CreateNoteDto note)
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var createdNote = await _service.CreateNote(note, userId);

            return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNote(long id)
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var deleted = await _service.DeleteNote(id, userId);

            if (!deleted) return NotFound();

            return NoContent();
        }
        
    }
}
