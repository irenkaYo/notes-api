using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotesApiDbContext _context;

    public NoteRepository(NotesApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetAllNotes(Guid userId)
    {
        List<Note> notes = await _context.Notes
            .Where(n => n.UserId == userId)
            .Include(u => u.User)
            .ToListAsync();
        
        return notes;
    }
    
    public async Task<Note?> GetNoteById(Guid noteId)
    {
        return await _context.Notes
            .Where(n => n.Id == noteId)
            .Include(u => u.User)
            .FirstOrDefaultAsync();
    }
    
    public async Task CreateNote(Note note)
    {
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNote(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(Note note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}