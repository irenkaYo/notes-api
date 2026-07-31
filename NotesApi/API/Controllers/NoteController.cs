using System.Security.Claims;
using Application.DTOs.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("notes")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    
    public NoteController(INoteService service)
    {
        _noteService = service;
    }
    
    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllNotes()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();
        
        var notes = await _noteService.GetAllNotes(userId);
        return Ok(notes);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        try
        {
            var note = await _noteService.GetNoteById(id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();
        
        var note = await _noteService.CreateNote(request, userId);
        return Ok(note);
    }

    [HttpPut("update/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest request, [FromRoute] Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();
        
        try
        {
            var note = await _noteService.UpdateNote(request, id, userId);
            return Ok(note);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        try
        {
            await _noteService.DeleteNote(id, userId);
            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    private bool TryGetUserId(out Guid userId)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdString, out userId);
    }
}