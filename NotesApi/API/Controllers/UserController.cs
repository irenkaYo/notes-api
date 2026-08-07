using System.Security.Claims;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = Application.DTOs.Requests.LoginRequest;
using RegisterRequest = Application.DTOs.Requests.RegisterRequest;

namespace API.Controllers;

[ApiController]
[Route("user/auth")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await _userService.RegisterUser(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _userService.LoginUser(request);
            return Ok(token);
        }
        catch (ArgumentException)
        {
            return Unauthorized();
        }
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        MeResponse response = new MeResponse(userIdString, username);
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var response = await _userService.UpdateRefreshToken(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}