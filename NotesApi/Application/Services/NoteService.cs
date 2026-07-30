using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Application.Services;

public class NoteService
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

    public async Task<NoteResponse> GetNoteById(Guid noteId)
    {
        Note note = await GetNoteOrThrow(noteId);
        var response = ConvertToDto(note);
        return response;
    }

    public async Task<NoteResponse> CreateNote(CreateNoteRequest request, Guid userId)
    {
        if (request.Text is null || request.Title is null)
            throw new Exception("Text and Text cannot be null");
        
        Note note = new Note(request.Title, request.Text, userId);
        await _noteRepository.CreateNote(note);
        var response = ConvertToDto(note);
        return response;
    }

    public async Task<NoteResponse> UpdateNote(UpdateNoteRequest request, Guid noteId)
    {
        Note note = await GetNoteOrThrow(noteId);
        await _noteRepository.UpdateNote(note);
        var response = ConvertToDto(note);
        return response;
    }

    public async Task DeleteNote(Guid noteId)
    {
        Note note = await GetNoteOrThrow(noteId);
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
        return new NoteResponse(note.Title, note.Text, note.CreatedAt);
    }

    private async Task<Note> GetNoteOrThrow(Guid noteId)
    {
        Note? note = await _noteRepository.GetNoteById(noteId);
        if  (note is null)
            throw new Exception("Note not found");
        return note;
    }
}