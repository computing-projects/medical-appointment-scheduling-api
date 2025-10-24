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
    private readonly UserData _userData;
    public AuthController(JwtTokenService jwtTokenService, SupabaseTokenService supabaseTokenService, IUsersRepository usersRepository, UserData userData)
    {
        _jwtTokenService = jwtTokenService;
        _supabaseTokenService = supabaseTokenService;
        _usersRepository = usersRepository;
        _userData = userData;
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

        // Aqui você define os valores da “global”
        _userData.UserId = user.Id;
        _userData.UserRole = user.Role;
        if (_userData.UserRole == SystemEnums.EUserRole.Client)
            _userData.MedicId = await _usersRepository.GetUserMedicId(user.Id);
        if (_userData.UserRole == SystemEnums.EUserRole.Medic)
            _userData.ClientId = await _usersRepository.GetUserClientId(user.Id);

        return Ok(new TokenDto { email = user.Email, token = newToken });
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        return hashString == passwordHash.ToLowerInvariant();
    }
}
