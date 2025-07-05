using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace medical_appointment_scheduling_api.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(255)]
        public string name { get; set; }

        [Required, MaxLength(255)]
        public string email { get; set; }

        [Required]
        public string password_hash { get; set; }

        [MaxLength(20)]
        public string? phone { get; set; }

        // Creatated to control whether the user is active or not
        public bool delete { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public SystemEnums.ETipoUsuario user_type { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;

    }

    public static class EncryptDecrypt
    {
        // Use a 32-byte key for AES-256
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("Your32CharLongEncryptionKey!123456");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("16CharInitVector!");

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
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
