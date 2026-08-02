using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;

namespace Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    
    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<List<NoteResponse>> GetAllNotes(Guid userId)
    {
        List<Note> notes = await _noteRepository.GetAllNotes(userId);
        var responses = ConvertListToDto(notes);
        return responses;
    }

    public async Task<NoteResponse> GetNoteById(Guid noteId, Guid userId)
    {
        Note note = await GetNoteOrThrow(noteId);
        
        EnsureUserOwnsNote(note, userId);
        
        var response = ConvertToDto(note);
        return response;
    }

    public async Task<NoteResponse> CreateNote(CreateNoteRequest request, Guid userId)
    {
        if (request.Text is null || request.Title is null)
            throw new Exception("Text and Title cannot be null");
        
        Note note = new Note(request.Title, request.Text, userId);
        await _noteRepository.CreateNote(note);
        var response = ConvertToDto(note);
        return response;
    }

    public async Task<NoteResponse> UpdateNote(UpdateNoteRequest request, Guid noteId, Guid userId)
    {
        Note note = await GetNoteOrThrow(noteId);
        
        EnsureUserOwnsNote(note, userId);
        
        if (request.Title is null)
            throw new Exception("Title cannot be null");
        
        note.Text = request.Text;
        note.Title = request.Title;
        await _noteRepository.UpdateNote(note);
        var response = ConvertToDto(note);
        return response;
    }

    public async Task DeleteNote(Guid noteId, Guid userId)
    {
        Note note = await GetNoteOrThrow(noteId);
        
        EnsureUserOwnsNote(note, userId);
        
        await _noteRepository.DeleteNote(note);
    }

    private List<NoteResponse> ConvertListToDto(List<Note> notes)
    {
        List<NoteResponse> responses = new List<NoteResponse>();
        foreach (var note in notes)
        {
            var response = ConvertToDto(note);
            responses.Add(response);
        }
        return responses;
    }
    
    private NoteResponse ConvertToDto(Note note)
    {
        return new NoteResponse(note.Id, note.Title, note.Text, note.CreatedAt);
    }

    private async Task<Note> GetNoteOrThrow(Guid noteId)
    {
        Note? note = await _noteRepository.GetNoteById(noteId);
        if  (note is null)
            throw new Exception("Note not found");
        return note;
    }
    
    private static void EnsureUserOwnsNote(Note note, Guid userId)
    {
        if (note.UserId != userId)
            throw new UnauthorizedAccessException();
    }
}