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

    [HttpPost("SupabaseLogin")]
    public async Task<IActionResult> Login([FromBody] LoginUser model)
    {
        if (string.IsNullOrEmpty(model.SupabaseAccessToken))
            return BadRequest("SupabaseAccessToken is required.");

        try
        {
            var user = await _supabaseTokenService.VerifyTokenAsync(model.SupabaseAccessToken);
            if (user == null)
                return Unauthorized("Invalid Supabase token.");

            var userEmail = user.Email;
            var uid = user.Id;

            var jwt = _jwtTokenService.GenerateToken(userEmail ?? uid);

            return Ok(new TokenDto()
            {
                firesbaseUIdToken = uid,
                email = userEmail ?? "",
                token = jwt
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid Supabase token.");
        }
    }

    [HttpPost("DirectLogin")]
    public async Task<IActionResult> LoginNormal([FromBody] LoginUser model)
    {
        var user = await _usersRepository.GetByEmailAsync(model.Username);
        if (user == null)
            return Unauthorized("Usu�rio ou senha inv�lidos.");

        var decryptedPassword = EncryptDecrypt.Decrypt(user.PasswordHash);

        if (!VerifyPassword(model.Password, user.PasswordHash))
            return Unauthorized("Usu�rio ou senha inv�lidos.");

        var newToken = _jwtTokenService.GenerateToken(user.Email);

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
