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

    }

    public static class EncryptDecrypt
    {
        // Use a 32-byte key for AES-256
        private static readonly byte[] Key = GetOrCreateKey(32);
        private static readonly byte[] IV = GetOrCreateIV(16);

        private static byte[] GetOrCreateKey(int sizeInBytes)
        {
            // For simplicity, generating on the fly. In production, load from secure config.
            return RandomNumberGenerator.GetBytes(sizeInBytes);
        }
    
        private static byte[] GetOrCreateIV(int sizeInBytes)
        {
            // For simplicity, generating on the fly. In production, load from secure config.
            return RandomNumberGenerator.GetBytes(sizeInBytes);
        }

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var buffer = Convert.FromBase64String(cipherText);
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
