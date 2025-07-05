using System.Security.Cryptography;
using System.Text;
using FirebaseAdmin.Auth;
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
    private readonly FirebaseTokenService _firebaseTokenService;
    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("FirebaseLogin")]
    public async Task<IActionResult> Login([FromBody] LoginUser model)
    {
        if (string.IsNullOrEmpty(model.FirebaseIdToken))
            return BadRequest("FirebaseIdToken is required.");

        try
        {
            var decodedToken = await _firebaseTokenService.VerifyTokenAsync(model.FirebaseIdToken);
            var uid = decodedToken.Uid;
            var userEmail = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"]?.ToString() : null;

            var jwt = _jwtTokenService.GenerateToken(userEmail ?? uid);

            return Ok(new TokenDto()
            {
                firesbaseUIdToken= uid,
                email = userEmail ?? "",
                token = jwt
            });
        }
        catch (FirebaseAuthException)
        {
            return Unauthorized("Invalid Firebase token.");
        }
    }

    [HttpPost("DirectLogin")]
    public async Task<IActionResult> LoginNormal([FromBody] LoginUser model)
    {
        var user = await _usersRepository.GetByEmailAsync(model.Username);
        if (user == null)
            return Unauthorized("Usuário ou senha inválidos.");

        var decryptedPassword = EncryptDecrypt.Decrypt(user.PasswordHash);

        if (!VerifyPassword(model.Password, user.PasswordHash))
            return Unauthorized("Usuário ou senha inválidos.");

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
