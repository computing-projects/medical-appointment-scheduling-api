using Supabase;

namespace medical_appointment_scheduling_api.Services
{
    public class ProfilePhotoService
    {
        private readonly Supabase.Client _supabase;
        private readonly IConfiguration _configuration;
        private const string BucketName = "profile-photos";

        public ProfilePhotoService(IConfiguration configuration)
        {
            _configuration = configuration;
            var supabaseUrl = _configuration["Supabase:Url"];
            // Use ServiceRoleKey for storage operations to bypass RLS
            var supabaseKey = _configuration["Supabase:ServiceRoleKey"] ?? _configuration["Supabase:AnonKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new InvalidOperationException("Supabase configuration is missing.");
            }

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = false
            };

            _supabase = new Supabase.Client(supabaseUrl, supabaseKey, options);
        }

        public async Task<string> UploadProfilePhotoAsync(int userId, byte[] imageData, string fileName)
        {
            try
            {
                await _supabase.InitializeAsync();

                // Create unique file path: userId/timestamp_filename
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var filePath = $"{userId}/{timestamp}_{fileName}";

                // Upload to Supabase Storage
                await _supabase.Storage
                    .From(BucketName)
                    .Upload(imageData, filePath, new Supabase.Storage.FileOptions
                    {
                        CacheControl = "3600",
                        Upsert = false
                    });

                // Get public URL
                var publicUrl = _supabase.Storage
                    .From(BucketName)
                    .GetPublicUrl(filePath);

                return publicUrl;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload profile photo: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteProfilePhotoAsync(string photoUrl)
        {
            try
            {
                await _supabase.InitializeAsync();

                // Extract file path from URL
                var uri = new Uri(photoUrl);
                var pathSegments = uri.AbsolutePath.Split('/');
                var filePath = string.Join("/", pathSegments.Skip(pathSegments.Length - 2));

                // Delete from Supabase Storage
                await _supabase.Storage
                    .From(BucketName)
                    .Remove(new List<string> { filePath });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

