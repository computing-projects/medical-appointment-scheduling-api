using System.Security.Cryptography;
using System.Text;
using Supabase.Gotrue;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;
using medical_appointment_scheduling_api.Repositories;
using medical_appointment_scheduling_api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly IUsersRepository _usersRepository;
    private readonly SupabaseTokenService _supabaseTokenService;
    public AuthController(JwtTokenService jwtTokenService, SupabaseTokenService supabaseTokenService, IUsersRepository usersRepository)
    {
        _jwtTokenService = jwtTokenService;
        _supabaseTokenService = supabaseTokenService;
        _usersRepository = usersRepository;
    }

    [HttpPost("DirectLogin")]
    public async Task<IActionResult> LoginNormal([FromBody] LoginUser model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid payload.");
        var user = await _usersRepository.GetByEmailAsync(model.Email);
        if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            return Unauthorized("Usuário ou senha inválidos.");
        var newToken = _jwtTokenService.GenerateToken(user.Email, user.Id);
        return Ok(new TokenDto { email = user.Email, token = newToken, role = user.Role});
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        return hashString == passwordHash.ToLowerInvariant();
    }
}
