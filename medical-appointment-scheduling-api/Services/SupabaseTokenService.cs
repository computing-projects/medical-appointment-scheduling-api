using Supabase;

namespace medical_appointment_scheduling_api.Services
{
    public class SupabaseTokenService
    {
        private readonly Supabase.Client _supabase;

        public SupabaseTokenService(IConfiguration configuration)
        {
            var supabaseUrl = configuration["Supabase:Url"];
            var supabaseKey = configuration["Supabase:AnonKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new InvalidOperationException("Supabase configuration is missing. Please check your appsettings.json file.");
            }

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = false
            };

            _supabase = new Supabase.Client(supabaseUrl, supabaseKey, options);
        }

        public async Task<dynamic?> VerifyTokenAsync(string accessToken)
        {
            try
            {
                // For now, we'll create a simple user object
                // In a real implementation, you would verify the token with Supabase
                return new { Id = "user_id", Email = "user@example.com" };
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid Supabase token.", ex);
            }
        }

        public async Task<Supabase.Gotrue.Session?> SignInWithEmailAsync(string email, string password)
        {
            try
            {
                await _supabase.InitializeAsync();
                var session = await _supabase.Auth.SignIn(email, password);
                
                if (session?.AccessToken == null)
                {
                    throw new UnauthorizedAccessException("Authentication failed - no session returned.");
                }
                
                return session;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException($"Invalid credentials: {ex.Message}", ex);
            }
        }

        public async Task<Supabase.Gotrue.Session?> SignUpWithEmailAsync(string email, string password)
        {
            try
            {
                await _supabase.InitializeAsync();
                var session = await _supabase.Auth.SignUp(email, password);
                
                if (session?.AccessToken == null)
                {
                    throw new InvalidOperationException("Sign up failed - no session returned.");
                }
                
                return session;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create user: {ex.Message}", ex);
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                // Placeholder implementation
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to sign out.", ex);
            }
        }
    }
}
