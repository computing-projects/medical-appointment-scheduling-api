using System.Security.Claims;
using medical_appointment_scheduling_api.Models.DTO;
using medical_appointment_scheduling_api.Repositories;
using medical_appointment_scheduling_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly SupabaseTokenService _supabaseTokenService;
    private readonly IUsersRepository _usersRepository;

    public AuthController(SupabaseTokenService supabaseTokenService, IUsersRepository usersRepository)
    {
        _supabaseTokenService = supabaseTokenService;
        _usersRepository = usersRepository;
    }

    [HttpPost("DirectLogin")]
    public async Task<IActionResult> DirectLogin([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid payload.");

        try
        {
            var session = await _supabaseTokenService.SignInWithEmailAsync(request.Email, request.Password);
            
            if (session?.AccessToken == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new TokenDto
            {
                email = session.User?.Email ?? request.Email,
                token = session.AccessToken
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Authentication failed: {ex.Message}");
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid payload.");

        try
        {
            var session = await _supabaseTokenService.SignUpWithEmailAsync(request.Email, request.Password);
            
            if (session?.AccessToken == null)
                return BadRequest("Registration failed.");

            return Ok(new TokenDto
            {
                email = session.User?.Email ?? request.Email,
                token = session.AccessToken
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Registration failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Registration failed: {ex.Message}");
        }
    }

    [HttpGet("CurrentUser")]
    [Authorize]
    public async Task<IActionResult> CurrentUser()
    {
        // Extract email from Supabase JWT
        var email = User.FindFirstValue("email") ?? User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return Unauthorized("Invalid token claims.");

        try
        {
            // Fetch user data from database
            var user = await _usersRepository.GetByEmailAsync(email);
            
            if (user == null)
                return NotFound($"User with email {email} not found in database.");

            return Ok(new CurrentUserDto
            {
                email = user.Email,
                name = user.Name,
                role = user.Role
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching user data: {ex.Message}");
        }
    }
}
