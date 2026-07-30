using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface INoteRepository
{
    public Task<List<Note>> GetAllNotes(Guid userId);
    public Task<Note?> GetNoteById(Guid noteId);
    public Task CreateNote(Note note);
    public Task UpdateNote(Note note);
    public Task DeleteNote(Note note);
}