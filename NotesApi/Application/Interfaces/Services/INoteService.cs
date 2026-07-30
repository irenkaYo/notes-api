using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface INoteService
{
    public Task<List<NoteResponse>> GetAllNotes(Guid userId);
    public Task<NoteResponse> GetNoteById(Guid noteId);
    public Task<NoteResponse> CreateNote(CreateNoteRequest request, Guid userId);
    public Task<NoteResponse> UpdateNote(UpdateNoteRequest request, Guid noteId);
    public Task DeleteNote(Guid noteId);
}