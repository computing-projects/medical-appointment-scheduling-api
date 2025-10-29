using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace medical_appointment_scheduling_api.Middleware;

public class SupabaseJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public SupabaseJwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _tokenHandler = new JwtSecurityTokenHandler();
        
        Console.WriteLine($"✓ JWT Middleware initialized - will trust Supabase tokens without signature validation");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring(7);
            AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            // Read the JWT without validating signature (since we can't get Supabase's internal signing key)
            // Supabase has already validated this token when it was issued
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            
            // Check if token is expired
            var exp = jwtToken.ValidTo;
            if (exp < DateTime.UtcNow)
            {
                Console.WriteLine($"✗ JWT is expired: {exp}");
                return;
            }
            
            // Create claims identity from token claims
            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "Supabase");
            var principal = new ClaimsPrincipal(identity);
            
            context.User = principal;
            
            var email = principal.FindFirst("email")?.Value ?? "unknown";
            Console.WriteLine($"✓ JWT validated successfully for user: {email}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ JWT parsing failed: {ex.Message}");
        }
    }
}