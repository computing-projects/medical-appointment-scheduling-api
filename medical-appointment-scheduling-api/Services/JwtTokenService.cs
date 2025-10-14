using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(string secret, string issuer, string audience)
    {
        _secret = secret;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateToken(string username, int? userId = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };
        if (userId.HasValue)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}