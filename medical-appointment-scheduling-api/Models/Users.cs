using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace medical_appointment_scheduling_api.Models
{
    public class Users
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("cep")]
        [RegularExpression(@"^\d{8}$")]
        public string Cep { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; }

        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTimeOffset DeletedAt { get; set; }

        [MaxLength(500)]
        [Column("profile_photo_url")]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(100)]
        [Column("city")]
        public string? City { get; set; }

        [MaxLength(2)]
        [Column("state")]
        public string? State { get; set; }

    }

    public static class PasswordHasher
    {
        public static string Hash(string plainText)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainText));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
